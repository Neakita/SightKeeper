using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Binary.Replication.DataSets.Detector;

internal sealed class DetectorAssetsReplicator
{
	public DetectorAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(DetectorDataSet dataSet, IReadOnlyCollection<PackableItemsAsset<PackableDetectorItem>> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var screenshot = _session.Screenshots[packableAsset.ScreenshotId];
			var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
			ReplicateItems(dataSet.TagsLibrary, asset, packableAsset.Items);
		}
	}

	private readonly ReplicationSession _session;

	private static void ReplicateItems(TagsLibrary<Tag> tagsLibrary, DetectorAsset asset, IReadOnlyCollection<PackableDetectorItem> packableItems)
	{
		foreach (var packableItem in packableItems)
		{
			var tag = tagsLibrary.Tags[packableItem.TagIndex];
			asset.CreateItem(tag, packableItem.Bounding);
		}
	}
}