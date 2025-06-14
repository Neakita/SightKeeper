using SightKeeper.Data.Model;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal static class Extensions
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
}