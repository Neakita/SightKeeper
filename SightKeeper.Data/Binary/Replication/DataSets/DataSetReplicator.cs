using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal abstract class DataSetReplicator<TDataSet>
	where TDataSet : DataSet, new()
{
	public TDataSet Replicate(PackableDataSet packable)
	{
		Guard.IsNotNull(Session.Games);
		var game = packable.GameId == null ? null : Session.Games[packable.GameId.Value];
		var dataSet = new TDataSet
		{
			Name = packable.Name,
			Description = packable.Description,
			Game = game
		};
		var screenshotsLookup = ReplicateScreenshots(dataSet.ScreenshotsLibrary, packable.Screenshots);
		ReplicateTags(dataSet.TagsLibrary, packable.GetTags());
		ReplicateAssets(
			dataSet.AssetsLibrary,
			packable.GetAssets(),
			screenshotId => screenshotsLookup[screenshotId]);
		if (packable.MaxScreenshotsWithoutAsset != null)
		{
			var screenshotsWithoutAssets =
				dataSet.ScreenshotsLibrary.Screenshots.Count - dataSet.AssetsLibrary.Assets.Count;
			Guard.IsLessThanOrEqualTo(screenshotsWithoutAssets, packable.MaxScreenshotsWithoutAsset.Value);
			dataSet.ScreenshotsLibrary.MaxLiabilityQuantity = packable.MaxScreenshotsWithoutAsset;
		}

		ReplicateWeights(dataSet.WeightsLibrary, packable.GetWeights());
		return dataSet;
	}

	protected ReplicationSession Session { get; }

	protected DataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session)
	{
		Session = session;
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	protected static Composition? ReplicateComposition(PackableComposition? composition) => composition switch
	{
		null => null,
		PackableFixedTransparentComposition fixedTransparent =>
			new FixedTransparentComposition(
				fixedTransparent.MaximumScreenshotsDelay,
				fixedTransparent.Opacities),
		PackableFloatingTransparentComposition floatingTransparent =>
			new FloatingTransparentComposition(
				floatingTransparent.MaximumScreenshotsDelay,
				floatingTransparent.SeriesDuration,
				floatingTransparent.PrimaryOpacity,
				floatingTransparent.MinimumOpacity),
		_ => throw new ArgumentOutOfRangeException(nameof(composition))
	};

	protected virtual Tag ReplicateTag(
		TagsLibrary library,
		PackableTag packedTag)
	{
		var tag = library.CreateTag(packedTag.Name);
		Session.Tags.Add((library.DataSet, packedTag.Id), tag);
		tag.Color = packedTag.Color;
		return tag;
	}

	protected abstract void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot);
	protected abstract Weights ReplicateWeights(WeightsLibrary library, PackableWeights weights);

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private ImmutableDictionary<Id, Screenshot> ReplicateScreenshots(ScreenshotsLibrary library,
		ImmutableArray<PackableScreenshot> screenshots)
	{
		var builder = ImmutableDictionary.CreateBuilder<Id, Screenshot>();
		foreach (var packedScreenshot in screenshots)
		{
			var screenshot = library.CreateScreenshot(packedScreenshot.CreationDate, packedScreenshot.Resolution,
				out var removedScreenshots);
			Guard.IsTrue(removedScreenshots.IsEmpty);
			_screenshotsDataAccess.AssociateId(screenshot, packedScreenshot.Id);
			builder.Add(packedScreenshot.Id, screenshot);
		}

		return builder.ToImmutable();
	}

	private void ReplicateTags(TagsLibrary library, ImmutableArray<PackableTag> tags)
	{
		foreach (var packedTag in tags)
			ReplicateTag(library, packedTag);
	}

	private void ReplicateAssets(AssetsLibrary library, ImmutableArray<PackableAsset> assets,
		Func<Id, Screenshot> getScreenshot)
	{
		foreach (var asset in assets)
			ReplicateAsset(library, asset, getScreenshot(asset.ScreenshotId));
	}

	private void ReplicateWeights(WeightsLibrary library, ImmutableArray<PackableWeights> packedWeights)
	{
		foreach (var item in packedWeights)
		{
			var weights = ReplicateWeights(library, item);
			Session.Weights.Add(item.Id, weights);
		}
	}
}