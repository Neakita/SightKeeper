using SightKeeper.Data.Replication.DataSets;
using SightKeeper.Data.Services;

namespace SightKeeper.Data.Replication;

internal class AppDataReplicator
{
	public AppDataReplicator(FileSystemImageDataAccess imageDataAccess)
	{
		_imageDataAccess = imageDataAccess;
	}

	public AppData Replicate(PackableAppData packable)
	{
		ReplicationSession session = new();
		ImageSetReplicator imageSetReplicator = new(session, _imageDataAccess);
		DataSetsReplicator dataSetReplicator = new(session);
		var imageSets = imageSetReplicator.ReplicateImageSets(packable.ImageSets).ToHashSet();
		var dataSets = dataSetReplicator.ReplicateDataSets(packable.DataSets).ToHashSet();
		return new AppData(imageSets, dataSets, packable.ApplicationSettings);
	}

	private readonly FileSystemImageDataAccess _imageDataAccess;
}