using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Replication.DataSets.Detector;

internal sealed class DetectorDataSetReplicator
{
	public DetectorDataSetReplicator(ReplicationSession session)
	{
		_assetsReplicator = new DetectorAssetsReplicator(session);
	}

	public DetectorDataSet ReplicateDataSet(PackableDetectorDataSet packableDataSet)
	{
		DetectorDataSet dataSet = new()
		{
			Name = packableDataSet.Name,
			Description = packableDataSet.Description
		};
		TagsReplicator.ReplicateTags(dataSet.TagsLibrary, packableDataSet.Tags);
		_assetsReplicator.ReplicateAssets(dataSet, packableDataSet.Assets);
		PlainWeightsReplicator.ReplicateWeights(dataSet.WeightsLibrary, dataSet.TagsLibrary, packableDataSet.Weights);
		return dataSet;
	}

	private readonly DetectorAssetsReplicator _assetsReplicator;
}