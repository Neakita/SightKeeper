using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

internal static class ImageExtensions
{
	public static ImageSet WithDomainRules(this ImageSet set)
	{
		return new DomainImageSet(set);
	}

	public static ImageSet WithTracking(this ImageSet set, ChangeListener listener)
	{
		return new TrackableImageSet(set, listener);
	}

	public static ImageSet WithLocking(this ImageSet set, Lock editingLock)
	{
		return new LockingImageSet(set, editingLock);
	}

	public static Image WithStreaming(this InMemoryImage image, FileSystemDataAccess dataAccess)
	{
		return new StreamableDataImage(image, dataAccess);
	}

	public static Image WithObservableAssets(this Image image)
	{
		return new ObservableAssetsImage(image);
	}

	public static ImageSet WithObservableImages(this ImageSet set)
	{
		return new ObservableImagesImageSet(set);
	}

	public static ImageSet WithNotifications(this ImageSet set)
	{
		return new NotifyingImageSet(set);
	}

	public static Id GetId(this Image image)
	{
		var inMemoryImage = image.UnWrapDecorator<InMemoryImage>();
		return inMemoryImage.Id;
	}
}