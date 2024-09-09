using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class MultiDataSetReplicator
{
	public MultiDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_classifierReplicator = new ClassifierDataSetReplicator(screenshotsDataAccess);
		_detectorReplicator = new DetectorDataSetReplicator(screenshotsDataAccess);
		_poser2DReplicator = new Poser2DDataSetReplicator(screenshotsDataAccess);
		_poser3DReplicator = new Poser3DDataSetReplicator(screenshotsDataAccess);
	}

	public HashSet<DataSet> Replicate(
		ImmutableArray<PackableDataSet> packableDataSets,
		ReplicationSession session,
		ImmutableDictionary<ushort, Weights>.Builder weightsLookupBuilder)
	{
		return packableDataSets.Select(dataSet => Replicate(dataSet, session, weightsLookupBuilder)).ToHashSet();
	}

	private readonly ClassifierDataSetReplicator _classifierReplicator;
	private readonly DetectorDataSetReplicator _detectorReplicator;
	private readonly Poser2DDataSetReplicator _poser2DReplicator;
	private readonly Poser3DDataSetReplicator _poser3DReplicator;

	private DataSet Replicate(PackableDataSet packableDataSet, ReplicationSession session, ImmutableDictionary<ushort, Weights>.Builder weightsLookupBuilder)
	{
		return packableDataSet switch
		{
			PackableClassifierDataSet classifierDataSet => _classifierReplicator.Replicate(classifierDataSet, session, weightsLookupBuilder),
			PackableDetectorDataSet detectorDataSet => _detectorReplicator.Replicate(detectorDataSet, session, weightsLookupBuilder),
			PackablePoser2DDataSet poser2DDataSet => _poser2DReplicator.Replicate(poser2DDataSet, session, weightsLookupBuilder),
			PackablePoser3DDataSet poser3DDataSet => _poser3DReplicator.Replicate(poser3DDataSet, session, weightsLookupBuilder),
			_ => throw new ArgumentOutOfRangeException(nameof(packableDataSet))
		};
	}
}