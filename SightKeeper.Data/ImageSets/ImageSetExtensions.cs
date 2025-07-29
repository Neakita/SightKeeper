using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal static class ImageSetExtensions
{
	public static StorableImageSet WithDomainRules(this StorableImageSet set)
	{
		return new StorableImageSetExtension(new DomainImageSet(set), set);
	}

	public static StorableImageSet WithTracking(this StorableImageSet set, ChangeListener listener)
	{
		return new TrackableImageSet(set, listener);
	}

	public static StorableImageSet WithLocking(this StorableImageSet set, Lock editingLock)
	{
		return new LockingImageSet(set, editingLock);
	}

	public static StorableImageSet WithObservableImages(this StorableImageSet set)
	{
		return new ObservableImagesImageSet(set);
	}

	public static StorableImageSet WithNotifications(this StorableImageSet set)
	{
		return new NotifyingImageSet(set);
	}

	public static StorableImageSet WithImagesDataRemoving(this StorableImageSet set)
	{
		return new ImagesDataRemovingImageSet(set);
	}

	public static StorableImageSet WithImagesDisposing(this StorableImageSet set)
	{
		return new DisposingImageSet(set);
	}
}