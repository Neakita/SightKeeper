using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser2D;

internal sealed class Poser2DAssetsReplicator
{
	public Poser2DAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(Poser2DDataSet dataSet, ImmutableArray<PackableItemsAsset<PackablePoser2DItem>> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var screenshot = _session.Screenshots[packableAsset.ScreenshotId];
			var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
			ReplicateItems(dataSet.TagsLibrary, asset, packableAsset.Items);
		}
	}

	private readonly ReplicationSession _session;

	private static void ReplicateItems(TagsLibrary<PoserTag> tagsLibrary, Poser2DAsset asset, ImmutableArray<PackablePoser2DItem> packableItems)
	{
		foreach (var packableItem in packableItems)
		{
			var tag = tagsLibrary.Tags[packableItem.TagIndex];
			var item = asset.CreateItem(tag, packableItem.Bounding);
			ReplicateKeyPoints(item, packableItem.KeyPoints);
		}
	}

	private static void ReplicateKeyPoints(Poser2DItem item, ImmutableArray<PackableKeyPoint> packableKeyPoints)
	{
		foreach (var packableKeyPoint in packableKeyPoints)
		{
			var tag = item.Tag.KeyPointTags[packableKeyPoint.TagIndex];
			item.CreateKeyPoint(tag, packableKeyPoint.Position);
		}
	}
}