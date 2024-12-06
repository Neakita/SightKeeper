using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Replication.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser3D;

internal sealed class Poser3DDataSetReplicator
{
	public Poser3DDataSetReplicator(ReplicationSession session)
	{
		_assetsReplicator = new Poser3DAssetsReplicator(session);
	}

	public Poser3DDataSet ReplicateDataSet(PackablePoser3DDataSet packableDataSet)
	{
		Poser3DDataSet dataSet = new()
		{
			Name = packableDataSet.Name,
			Description = packableDataSet.Description
		};
		PoserTagsReplicator.ReplicateTags(dataSet.TagsLibrary, packableDataSet.Tags);
		_assetsReplicator.ReplicateAssets(dataSet, packableDataSet.Assets);
		PoserWeightsReplicator.ReplicateWeights(dataSet.WeightsLibrary, dataSet.TagsLibrary, packableDataSet.Weights);
		return dataSet;
	}

	private readonly Poser3DAssetsReplicator _assetsReplicator;
}