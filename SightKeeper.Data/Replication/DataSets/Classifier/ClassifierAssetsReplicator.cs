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
			var image = _session.Images[packableAsset.ImageId];
			var asset = dataSet.AssetsLibrary.MakeAsset(image);
			asset.Tag = dataSet.TagsLibrary.Tags[packableAsset.TagIndex];
		}
	}

	private readonly ReplicationSession _session;
}