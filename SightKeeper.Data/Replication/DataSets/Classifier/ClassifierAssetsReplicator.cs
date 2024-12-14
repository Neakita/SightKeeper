using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Replication.DataSets.Classifier;

internal sealed class ClassifierAssetsReplicator
{
	public ClassifierAssetsReplicator(ReplicationSession session)
	{
		_session = session;
	}

	public void ReplicateAssets(ClassifierDataSet dataSet, IReadOnlyCollection<PackableClassifierAsset> packableAssets)
	{
		foreach (var packableAsset in packableAssets)
		{
			var screenshot = _session.Screenshots[packableAsset.ScreenshotId];
			var asset = dataSet.AssetsLibrary.MakeAsset(screenshot);
			asset.Tag = dataSet.TagsLibrary.Tags[packableAsset.TagIndex];
			asset.Usage = packableAsset.Usage;
		}
	}

	private readonly ReplicationSession _session;
}