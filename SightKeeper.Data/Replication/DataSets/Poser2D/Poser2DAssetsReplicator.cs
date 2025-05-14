using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser2D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Replication.DataSets.Poser2D;

internal sealed class Poser2DAssetsReplicator
{
	public Poser2DAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(Poser2DDataSet dataSet, IReadOnlyCollection<PackableItemsAsset<PackablePoser2DItem>> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var image = _session.Images[packableAsset.ImageId];
			var asset = dataSet.AssetsLibrary.MakeAsset(image);
			ReplicateItems(dataSet.TagsLibrary, asset, packableAsset.Items);
		}
	}

	private readonly ReplicationSession _session;

	private static void ReplicateItems(TagsLibrary<PoserTag> tagsLibrary, Poser2DAsset asset, IReadOnlyCollection<PackablePoser2DItem> packableItems)
	{
		foreach (var packableItem in packableItems)
		{
			var tag = tagsLibrary.Tags[packableItem.TagIndex];
			var item = asset.MakeItem(tag, packableItem.Bounding);
			ReplicateKeyPoints(item, packableItem.KeyPoints);
		}
	}

	private static void ReplicateKeyPoints(Poser2DItem item, IReadOnlyCollection<PackableKeyPoint> packableKeyPoints)
	{
		foreach (var packableKeyPoint in packableKeyPoints)
		{
			var tag = item.Tag.KeyPointTags[packableKeyPoint.TagIndex];
			var keyPoint = item.MakeKeyPoint(tag);
			keyPoint.Position = packableKeyPoint.Position;
		}
	}
}