using Serilog;
using SightKeeper.Data.Model;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Replication;

internal sealed class ImageSetReplicator
{
	public ImageSetReplicator(ReplicationSession session, WriteIdRepository<Image> imageRepository, ILogger logger)
	{
		_session = session;
		_imageRepository = imageRepository;
		_logger = logger;
	}

	public IEnumerable<ImageSet> ReplicateImageSets(IEnumerable<PackableImageSet> packableSets)
	{
		return packableSets.Select(ReplicateImageSet);
	}

	private readonly ReplicationSession _session;
	private readonly WriteIdRepository<Image> _imageRepository;
	private readonly ILogger _logger;

	private ImageSet ReplicateImageSet(PackableImageSet packableSet)
	{
		ImageSet set = new()
		{
			Name = packableSet.Name,
			Description = packableSet.Description
		};
		foreach (var packableImage in packableSet.Images)
		{
			var image = set.CreateImage(packableImage.CreationTimestamp, packableImage.Size);
			var imageId = packableImage.Id;
			try
			{
				_imageRepository.AssociateId(image, imageId);
			}
			catch (ArgumentException exception)
			{
				_logger.Warning(exception, "An exception was thrown when trying to associate an image object with the id {Id}. This could have happened when the image file was deleted, but the Image set was not saved with the deleted image", imageId);
				set.RemoveImage(image);
				continue;
			}
			_session.Images.Add(imageId, image);
		}
		return set;
	}
}