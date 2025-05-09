using SightKeeper.Data.Model.DataSets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Replication.DataSets.Classifier;

internal sealed class ClassifierDataSetReplicator
{
	public ClassifierDataSetReplicator(ReplicationSession session)
	{
		_assetsReplicator = new ClassifierAssetsReplicator(session);
	}

	public ClassifierDataSet ReplicateDataSet(PackableClassifierDataSet packableDataSet)
	{
		ClassifierDataSet dataSet = new()
		{
			Name = packableDataSet.Name,
			Description = packableDataSet.Description
		};
		TagsReplicator.ReplicateTags(dataSet.TagsLibrary, packableDataSet.Tags);
		_assetsReplicator.ReplicateAssets(dataSet, packableDataSet.Assets);
		WeightsReplicator.ReplicateWeights(dataSet.WeightsLibrary, dataSet.TagsLibrary, packableDataSet.Weights);
		return dataSet;
	}

	private readonly ClassifierAssetsReplicator _assetsReplicator;
}