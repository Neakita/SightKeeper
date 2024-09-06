using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class Poser2DDataSetReplicator : PoserDataSetReplicator
{
	public Poser2DDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}

	protected override Poser2DDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new Poser2DDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, TagGetter getTag)
	{
		var typedLibrary = (AssetsLibrary<Poser2DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser2DItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<Poser2DAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser2DTag)getTag(packedItem.TagId);
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => keyPoint.Position).ToImmutableList(),
				packedItem.NumericProperties);
		}
	}

	protected override void ReplicateWeights(WeightsLibrary library, PackableWeights weights, TagGetter getTag)
	{
		var typedLibrary = (WeightsLibrary<Poser2DTag, KeyPointTag2D>)library;
		var typedWeights = (PackablePoserWeights)weights;
		var tags = GetTags(typedWeights, getTag);
		typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}

	private static IEnumerable<(Poser2DTag, IEnumerable<KeyPointTag2D>)> GetTags(PackablePoserWeights weights, TagGetter getTag)
	{
		foreach (var (tagId, keyPointTagIds) in weights.Tags)
		{
			var tag = (Poser2DTag)getTag(tagId);
			var keyPointTags = GetKeyPointTags(tagId, keyPointTagIds, getTag);
			yield return (tag, keyPointTags);
		}
	}

	private static IEnumerable<KeyPointTag2D> GetKeyPointTags(byte tagId, IEnumerable<byte> keyPointTagIds, TagGetter getTag)
	{
		foreach (var keyPointTagId in keyPointTagIds)
			yield return (KeyPointTag2D)getTag(tagId, keyPointTagId);
	}
}