using CommunityToolkit.Diagnostics;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets;

public abstract class ImageSetDeleter
{
	public required WriteRepository<ImageSet> ImageSetsRepository { get; init; }
	public required WriteImageDataAccess ImageDataAccess { get; init; }

	public static bool CanDelete(ImageSet set)
	{
		return set.Images.All(image => image.Assets.Count == 0);
	}

	public virtual void Delete(ImageSet set)
	{
		var canDelete = CanDelete(set);
		Guard.IsTrue(canDelete);
		ImageSetsRepository.Remove(set);
		DeleteImagesData(set);
	}

	private void DeleteImagesData(ImageSet set)
	{
		foreach (var image in set.Images)
			ImageDataAccess.DeleteImageData(image);
	}
}