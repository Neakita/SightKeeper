using SightKeeper.Data.ImageSets.Decorators;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal static class ImageSetExtensions
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

	public static ImageSet WithObservableImages(this ImageSet set)
	{
		return new ObservableImagesImageSet(set);
	}

	public static ImageSet WithNotifications(this ImageSet set)
	{
		return new NotifyingImageSet(set);
	}

	public static ImageSet WithImagesDataRemoving(this ImageSet set)
	{
		return new ImagesDataRemovingImageSet(set);
	}

	public static ImageSet WithImagesDisposing(this ImageSet set)
	{
		return new DisposingImageSet(set);
	}

	public static ImageSet WithLookupperPopulator(this ImageSet set, ImageLookupperPopulator populator)
	{
		return new PopulateImageLookupperImageSet(set, populator);
	}
}