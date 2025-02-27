using SightKeeper.Data.Model;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Replication;

internal sealed class ImageSetReplicator
{
	public ImageSetReplicator(ReplicationSession session, FileSystemImageDataAccess imageDataAccess)
	{
		_session = session;
		_imageDataAccess = imageDataAccess;
	}

	public IEnumerable<ImageSet> ReplicateImageSets(IEnumerable<PackableImageSet> packableSets)
	{
		return packableSets.Select(ReplicateImageSet);
	}

	private readonly ReplicationSession _session;
	private readonly FileSystemImageDataAccess _imageDataAccess;

	private ImageSet ReplicateImageSet(PackableImageSet packableSet)
	{
		ImageSet set = new()
		{
			Name = packableSet.Name,
			Description = packableSet.Description
		};
		foreach (var packableImage in packableSet.Images)
		{
			var image = set.CreateImage(packableImage.CreationTimestamp, packableImage.Image);
			var imageId = packableImage.Id;
			_session.Images.Add(imageId, image);
			_imageDataAccess.AssociateId(image, imageId);
		}
		return set;
	}
}