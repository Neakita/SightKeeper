using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class Poser3DDataSetReplicator : PoserDataSetReplicator
{
	public Poser3DDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess) : base(screenshotsDataAccess)
	{
	}


	protected override Poser3DDataSet CreateDataSet(string name, string description, Game? game, Composition? composition)
	{
		return new Poser3DDataSet
		{
			Name = name,
			Description = description,
			Game = game,
			Composition = composition
		};
	}

	protected override void ReplicateAsset(AssetsLibrary library, PackableAsset packedAsset, Screenshot screenshot, TagGetter getTag)
	{
		var typedLibrary = (AssetsLibrary<Poser3DAsset>)library;
		var typedPackedAsset = (PackableItemsAsset<PackablePoser3DItem>)packedAsset;
		var asset = typedLibrary.MakeAsset((Screenshot<Poser3DAsset>)screenshot);
		foreach (var packedItem in typedPackedAsset.Items)
		{
			var itemTag = (Poser3DTag)getTag(packedItem.TagId);
			asset.CreateItem(
				itemTag,
				packedItem.Bounding,
				packedItem.KeyPoints.Select(keyPoint => new KeyPoint3D(keyPoint.Position, keyPoint.IsVisible)).ToImmutableList(),
				packedItem.NumericProperties,
				packedItem.BooleanProperties);
		}
	}

	protected override void ReplicateWeights(WeightsLibrary library, PackableWeights weights, TagGetter getTag)
	{
		var typedLibrary = (WeightsLibrary<Poser3DTag, KeyPointTag3D>)library;
		var typedWeights = (PackablePoserWeights)weights;
		var tags = GetTags(typedWeights, getTag);
		typedLibrary.CreateWeights(weights.CreationDate, weights.ModelSize, weights.Metrics, weights.Resolution, tags);
	}

	private static IEnumerable<(Poser3DTag, IEnumerable<KeyPointTag3D>)> GetTags(PackablePoserWeights weights, TagGetter getTag)
	{
		foreach (var (tagId, keyPointTagIds) in weights.Tags)
		{
			var tag = (Poser3DTag)getTag(tagId);
			var keyPointTags = GetKeyPointTags(tagId, keyPointTagIds, getTag);
			yield return (tag, keyPointTags);
		}
	}

	private static IEnumerable<KeyPointTag3D> GetKeyPointTags(byte tagId, IEnumerable<byte> keyPointTagIds, TagGetter getTag)
	{
		foreach (var keyPointTagId in keyPointTagIds)
			yield return (KeyPointTag3D)getTag(tagId, keyPointTagId);
	}
}