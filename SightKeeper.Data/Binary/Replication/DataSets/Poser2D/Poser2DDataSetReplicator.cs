using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Replication.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Replication.DataSets.Poser2D;

internal sealed class Poser2DDataSetReplicator
{
	public Poser2DDataSetReplicator(ReplicationSession session)
	{
		_assetsReplicator = new Poser2DAssetsReplicator(session);
	}

	public Poser2DDataSet ReplicateDataSet(PackablePoser2DDataSet packableDataSet)
	{
		Poser2DDataSet dataSet = new()
		{
			Name = packableDataSet.Name,
			Description = packableDataSet.Description
		};
		PoserTagsReplicator.ReplicateTags(dataSet.TagsLibrary, packableDataSet.Tags);
		_assetsReplicator.ReplicateAssets(dataSet, packableDataSet.Assets);
		PoserWeightsReplicator.ReplicateWeights(dataSet.WeightsLibrary, dataSet.TagsLibrary, packableDataSet.Weights);
		return dataSet;
	}

	private readonly Poser2DAssetsReplicator _assetsReplicator;
}