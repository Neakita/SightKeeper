using SightKeeper.Data.Model.DataSets;
using SightKeeper.Data.Replication.DataSets.Classifier;
using SightKeeper.Data.Replication.DataSets.Detector;
using SightKeeper.Data.Replication.DataSets.Poser2D;
using SightKeeper.Data.Replication.DataSets.Poser3D;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Replication.DataSets;

internal sealed class DataSetsReplicator
{
	public DataSetsReplicator(ReplicationSession session)
	{
		_classifierReplicator = new ClassifierDataSetReplicator(session);
		_detectorReplicator = new DetectorDataSetReplicator(session);
		_poser2DReplicator = new Poser2DDataSetReplicator(session);
		_poser3DReplicator = new Poser3DDataSetReplicator(session);
	}

	public IEnumerable<DataSet> ReplicateDataSets(IEnumerable<PackableDataSet> packableDataSets)
	{
		return packableDataSets.Select(ReplicateDataSet);
	}

	private readonly ClassifierDataSetReplicator _classifierReplicator;
	private readonly DetectorDataSetReplicator _detectorReplicator;
	private readonly Poser2DDataSetReplicator _poser2DReplicator;
	private readonly Poser3DDataSetReplicator _poser3DReplicator;

	private DataSet ReplicateDataSet(PackableDataSet packableDataSet) => packableDataSet switch
	{
		PackableClassifierDataSet classifierDataSet => _classifierReplicator.ReplicateDataSet(classifierDataSet),
		PackableDetectorDataSet detectorDataSet => _detectorReplicator.ReplicateDataSet(detectorDataSet),
		PackablePoser2DDataSet poser2DDataSet => _poser2DReplicator.ReplicateDataSet(poser2DDataSet),
		PackablePoser3DDataSet poser3DDataSet => _poser3DReplicator.ReplicateDataSet(poser3DDataSet),
		_ => throw new ArgumentOutOfRangeException(nameof(packableDataSet))
	};
}