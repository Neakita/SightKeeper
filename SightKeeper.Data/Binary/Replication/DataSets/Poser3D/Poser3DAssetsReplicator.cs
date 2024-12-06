using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser3D;

internal sealed class Poser3DAssetsReplicator
{
	public Poser3DAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(Poser3DDataSet dataSet, ImmutableArray<PackableItemsAsset<PackablePoser3DItem>> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var screenshot = _session.Screenshots[packableAsset.ScreenshotId];
			var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
			ReplicateItems(dataSet.TagsLibrary, asset, packableAsset.Items);
		}
	}

	private readonly ReplicationSession _session;

	private static void ReplicateItems(TagsLibrary<PoserTag> tagsLibrary, Poser3DAsset asset, ImmutableArray<PackablePoser3DItem> packableItems)
	{
		foreach (var packableItem in packableItems)
		{
			var tag = tagsLibrary.Tags[packableItem.TagIndex];
			var item = asset.CreateItem(tag, packableItem.Bounding);
			ReplicateKeyPoints(item, packableItem.KeyPoints);
		}
	}

	private static void ReplicateKeyPoints(Poser3DItem item, ImmutableArray<PackableKeyPoint3D> packableKeyPoints)
	{
		foreach (var packableKeyPoint in packableKeyPoints)
		{
			var tag = item.Tag.KeyPointTags[packableKeyPoint.TagIndex];
			var keyPoint = item.CreateKeyPoint(tag, packableKeyPoint.Position);
			keyPoint.IsVisible = packableKeyPoint.IsVisible;
		}
	}
}