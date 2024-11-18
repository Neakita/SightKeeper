using System.Collections.Immutable;
using SightKeeper.Data.Binary.Model.DataSets;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Replication.DataSets;

internal sealed class MultiDataSetReplicator
{
	public MultiDataSetReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess, ReplicationSession session)
	{
		_classifierReplicator = new ClassifierDataSetReplicator(screenshotsDataAccess, session);
		_detectorReplicator = new DetectorDataSetReplicator(screenshotsDataAccess, session);
		_poser2DReplicator = new Poser2DDataSetReplicator(screenshotsDataAccess, session);
		_poser3DReplicator = new Poser3DDataSetReplicator(screenshotsDataAccess, session);
	}

	public HashSet<DataSet> Replicate(
		ImmutableArray<PackableDataSet> packableDataSets)
	{
		return packableDataSets.Select(Replicate).ToHashSet();
	}

	private readonly ClassifierDataSetReplicator _classifierReplicator;
	private readonly DetectorDataSetReplicator _detectorReplicator;
	private readonly Poser2DDataSetReplicator _poser2DReplicator;
	private readonly Poser3DDataSetReplicator _poser3DReplicator;

	private DataSet Replicate(PackableDataSet packableDataSet)
	{
		return packableDataSet switch
		{
			PackableClassifierDataSet classifierDataSet => _classifierReplicator.Replicate(classifierDataSet),
			PackableDetectorDataSet detectorDataSet => _detectorReplicator.Replicate(detectorDataSet),
			PackablePoser2DDataSet poser2DDataSet => _poser2DReplicator.Replicate(poser2DDataSet),
			PackablePoser3DDataSet poser3DDataSet => _poser3DReplicator.Replicate(poser3DDataSet),
			_ => throw new ArgumentOutOfRangeException(nameof(packableDataSet))
		};
	}
}