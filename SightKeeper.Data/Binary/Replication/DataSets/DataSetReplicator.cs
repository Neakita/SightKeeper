using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal abstract class DataSetReplicator
{
	public DataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public DataSet Replicate(PackableDataSet packed, ReplicationSession session)
	{
		Guard.IsNotNull(session.Games);
		var game = packed.GameId == null ? null : session.Games[packed.GameId.Value];
		var composition = ReplicateComposition(packed.Composition);
		var dataSet = CreateDataSet(packed.Name, packed.Description, game, composition);
		var screenshotsLookup = ReplicateScreenshots(dataSet.ScreenshotsLibrary, packed.Screenshots);
		ReplicateTags(dataSet.TagsLibrary, packed.GetTags(), session);
		ReplicateAssets(
			dataSet.AssetsLibrary,
			packed.GetAssets(),
			screenshotId => screenshotsLookup[screenshotId],
			session);
		if (packed.MaxScreenshotsWithoutAsset != null)
		{
			var screenshotsWithoutAssets = dataSet.ScreenshotsLibrary.Screenshots.Count - dataSet.AssetsLibrary.Assets.Count;
			Guard.IsLessThanOrEqualTo(screenshotsWithoutAssets, packed.MaxScreenshotsWithoutAsset.Value);
			dataSet.ScreenshotsLibrary.MaxQuantity = packed.MaxScreenshotsWithoutAsset;
		}
		ReplicateWeights(dataSet.WeightsLibrary, packed.GetWeights(), session);
		return dataSet;
	}

	protected abstract DataSet CreateDataSet(string name, string description, Game? game, Composition? composition);

	protected virtual Tag ReplicateTag(
		TagsLibrary library,
		PackableTag packedTag,
		ReplicationSession session)
	{
		var tag = library.CreateTag(packedTag.Name);
		session.Tags.Add((library.DataSet, packedTag.Id), tag);
		tag.Color = packedTag.Color;
		return tag;
	}

	protected abstract void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, ReplicationSession session);
	protected abstract Weights ReplicateWeights(WeightsLibrary library, PackableWeights weights, ReplicationSession session);

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;

	private static Composition? ReplicateComposition(PackableComposition? composition)
	{
		return composition switch
		{
			null => null,
			PackableTransparentComposition transparentComposition =>
				new TransparentComposition(
					transparentComposition.MaximumScreenshotsDelay,
					transparentComposition.Opacities),
			_ => throw new ArgumentOutOfRangeException(nameof(composition))
		};
	}

	private ImmutableDictionary<Id, Screenshot> ReplicateScreenshots(ScreenshotsLibrary library, ImmutableArray<PackableScreenshot> screenshots)
	{
		var builder = ImmutableDictionary.CreateBuilder<Id, Screenshot>();
		foreach (var packedScreenshot in screenshots)
		{
			var screenshot = library.CreateScreenshot(packedScreenshot.CreationDate, packedScreenshot.Resolution, out var removedScreenshots);
			Guard.IsTrue(removedScreenshots.IsEmpty);
			_screenshotsDataAccess.AssociateId(screenshot, packedScreenshot.Id);
			builder.Add(packedScreenshot.Id, screenshot);
		}
		return builder.ToImmutable();
	}

	private void ReplicateTags(TagsLibrary library, ImmutableArray<PackableTag> tags, ReplicationSession session)
	{
		foreach (var packedTag in tags)
			ReplicateTag(library, packedTag, session);
	}

	private void ReplicateAssets(AssetsLibrary library, ImmutableArray<PackableAsset> assets, Func<Id, Screenshot> getScreenshot, ReplicationSession session)
	{
		foreach (var asset in assets)
			ReplicateAsset(library, asset, getScreenshot(asset.ScreenshotId), session);
	}

	private void ReplicateWeights(WeightsLibrary library, ImmutableArray<PackableWeights> packedWeights, ReplicationSession session)
	{
		foreach (var item in packedWeights)
		{
			var weights = ReplicateWeights(library, item, session);
			session.Weights.Add(item.Id, weights);
		}
	}
}