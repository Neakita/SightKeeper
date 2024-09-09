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
	protected delegate Tag TagGetter(byte TagId, byte? KeyPointTagId = null);
	
	public DataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public DataSet Replicate(PackableDataSet packed, ReplicationSession session, ImmutableDictionary<ushort, Weights>.Builder weightsLookupBuilder)
	{
		Guard.IsNotNull(session.Games);
		var game = packed.GameId == null ? null : session.Games[packed.GameId.Value];
		var composition = ReplicateComposition(packed.Composition);
		var dataSet = CreateDataSet(packed.Name, packed.Description, game, composition);
		var screenshotsLookup = ReplicateScreenshots(dataSet.ScreenshotsLibrary, packed.Screenshots);
		var tagsLookup = ReplicateTags(dataSet.TagsLibrary, packed.GetTags());
		TagGetter getTag = (tagId, keyPointTagId) => tagsLookup[(tagId, keyPointTagId)];
		ReplicateAssets(
			dataSet.AssetsLibrary,
			packed.GetAssets(),
			screenshotId => screenshotsLookup[screenshotId],
			getTag);
		if (packed.MaxScreenshotsWithoutAsset != null)
		{
			var screenshotsWithoutAssets = dataSet.ScreenshotsLibrary.Screenshots.Count - dataSet.AssetsLibrary.Assets.Count;
			Guard.IsLessThanOrEqualTo(screenshotsWithoutAssets, packed.MaxScreenshotsWithoutAsset.Value);
			dataSet.ScreenshotsLibrary.MaxQuantity = packed.MaxScreenshotsWithoutAsset;
		}
		ReplicateWeights(dataSet.WeightsLibrary, packed.GetWeights(), getTag, weightsLookupBuilder);
		return dataSet;
	}

	protected abstract DataSet CreateDataSet(string name, string description, Game? game, Composition? composition);

	protected virtual Tag ReplicateTag(
		TagsLibrary library,
		PackableTag packed,
		ImmutableDictionary<(byte, byte?), Tag>.Builder lookupBuilder)
	{
		byte insertIndex = (byte)library.Tags.Count;
		var tag = library.CreateTag(packed.Name);
		tag.Color = packed.Color;
		lookupBuilder.Add((insertIndex, null), tag);
		return tag;
	}

	protected abstract void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, TagGetter getTag);
	protected abstract Weights ReplicateWeights(WeightsLibrary library, PackableWeights weights, TagGetter getTag);

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

	private ImmutableDictionary<(byte, byte?), Tag> ReplicateTags(TagsLibrary library, ImmutableArray<PackableTag> tags)
	{
		var lookupBuilder = ImmutableDictionary.CreateBuilder<(byte, byte?), Tag>();
		foreach (var tag in tags)
			ReplicateTag(library, tag, lookupBuilder);
		return lookupBuilder.ToImmutable();
	}

	private void ReplicateAssets(AssetsLibrary library, ImmutableArray<PackableAsset> assets, Func<Id, Screenshot> getScreenshot, TagGetter getTag)
	{
		foreach (var asset in assets)
			ReplicateAsset(library, asset, getScreenshot(asset.ScreenshotId), getTag);
	}

	private void ReplicateWeights(WeightsLibrary library, ImmutableArray<PackableWeights> packedWeights, TagGetter getTag, ImmutableDictionary<ushort, Weights>.Builder weightsLookupBuilder)
	{
		foreach (var item in packedWeights)
		{
			var weights = ReplicateWeights(library, item, getTag);
			weightsLookupBuilder.Add(item.Id, weights);
		}
	}
}