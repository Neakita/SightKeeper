using SightKeeper.Data.Binary.Replication.DataSets;
using SightKeeper.Data.Binary.Replication.Profiles;
using SightKeeper.Data.Binary.Services;

namespace SightKeeper.Data.Binary.Replication;

internal class AppDataReplicator
{
	public AppDataReplicator(FileSystemScreenshotsDataAccess screenshotsDataAccess)
	{
		_screenshotsDataAccess = screenshotsDataAccess;
	}

	public AppData Replicate(PackableAppData packed)
	{
		ReplicationSession session = new();
		var games = GameReplicator.Replicate(packed.Games, session);
		MultiDataSetReplicator dataSetReplicator = new(_screenshotsDataAccess, session);
		var dataSets = dataSetReplicator.Replicate(packed.DataSets);
		var profiles = new ProfileReplicator(session).Replicate(packed.Profiles).ToHashSet();
		return new AppData(games, dataSets, profiles, packed.ApplicationSettings);
	}

	private readonly FileSystemScreenshotsDataAccess _screenshotsDataAccess;
}