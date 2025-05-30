using Serilog;
using SightKeeper.Data.Replication.DataSets;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Replication;

internal class AppDataReplicator
{
	public AppDataReplicator(WriteIdRepository<Image> imageRepository, ILogger logger)
	{
		_imageRepository = imageRepository;
		_logger = logger;
	}

	public AppData Replicate(PackableAppData packable)
	{
		ReplicationSession session = new();
		ImageSetReplicator imageSetReplicator = new(session, _imageRepository, _logger.ForContext<ImageSetReplicator>());
		DataSetsReplicator dataSetReplicator = new(session);
		var imageSets = imageSetReplicator.ReplicateImageSets(packable.ImageSets).ToHashSet();
		var dataSets = dataSetReplicator.ReplicateDataSets(packable.DataSets).ToHashSet();
		return new AppData(imageSets, dataSets);
	}

	private readonly WriteIdRepository<Image> _imageRepository;
	private readonly ILogger _logger;
}