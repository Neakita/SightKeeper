using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Poser3D;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Replication.DataSets.Poser3D;

internal sealed class Poser3DAssetsReplicator
{
	public Poser3DAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(Poser3DDataSet dataSet, IReadOnlyCollection<PackableItemsAsset<PackablePoser3DItem>> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var image = _session.Images[packableAsset.ImageId];
			var asset = dataSet.AssetsLibrary.MakeAsset(image);
			ReplicateItems(dataSet.TagsLibrary, asset, packableAsset.Items);
		}
	}

	private readonly ReplicationSession _session;

	private static void ReplicateItems(TagsLibrary<PoserTag> tagsLibrary, Poser3DAsset asset, IReadOnlyCollection<PackablePoser3DItem> packableItems)
	{
		foreach (var packableItem in packableItems)
		{
			var tag = tagsLibrary.Tags[packableItem.TagIndex];
			var item = asset.CreateItem(tag, packableItem.Bounding);
			ReplicateKeyPoints(item, packableItem.KeyPoints);
		}
	}

	private static void ReplicateKeyPoints(Poser3DItem item, IReadOnlyCollection<PackableKeyPoint3D> packableKeyPoints)
	{
		foreach (var packableKeyPoint in packableKeyPoints)
		{
			var tag = item.Tag.KeyPointTags[packableKeyPoint.TagIndex];
			var keyPoint = item.CreateKeyPoint(tag, packableKeyPoint.Position);
			keyPoint.IsVisible = packableKeyPoint.IsVisible;
		}
	}
}