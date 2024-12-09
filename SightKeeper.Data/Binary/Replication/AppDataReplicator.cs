using SightKeeper.Data.Binary.Replication.DataSets;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Replication;

internal class AppDataReplicator
{
	public AppDataReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public AppData Replicate(PackableAppData packable)
	{
		ReplicationSession session = new();
		ScreenshotsLibraryReplicator screenshotsReplicator = new(session, _screenshotsDataAccess);
		DataSetsReplicator dataSetReplicator = new(session);
		var screenshotsLibraries = screenshotsReplicator.ReplicateScreenshotsLibraries(packable.ScreenshotsLibraries).ToHashSet();
		var dataSets = dataSetReplicator.ReplicateDataSets(packable.DataSets).ToHashSet();
		return new AppData(screenshotsLibraries, dataSets, packable.ApplicationSettings);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}