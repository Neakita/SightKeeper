namespace SightKeeper.Data.ImageSets;

public sealed class StorableImageSetWrapper(ChangeListener changeListener, Lock editingLock) : ImageSetWrapper
{
	public StorableImageSet Wrap(StorableImageSet set)
	{
		return set

			// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
			// and then tracking decorator will send another notification.
			.WithTracking(changeListener)

			// Locking of domain rules can be relatively computationally heavy,
			// for example when removing images range every image should be checked if it is used by some asset,
			// so locking appears only after domain rules validated.
			.WithLocking(editingLock)

			// Images data removing could be expansive (we can remove hundreds or thousands of images in one call, or do that often),
			// and there is no need in lock because lock should affect AppData only,
			// not the image files,
			// so it shouldn't be locked
			.WithImagesDataRemoving()

			// Changes shouldn't be observed if they aren't valid,
			// so it should be behind domain rules
			.WithObservableImages()

			// We shouldn't dispose images if domain rule is violated,
			// so this should be behind domain rules
			.WithImagesDisposing()

			// If domain rule is violated and throws an exception,
			// it should fail as fast as possible and have smaller stack strace
			.WithDomainRules()

			// INPC interface should be exposed to consumer,
			// so he can type test and cast it,
			// so it should be the outermost layer
			.WithNotifications();
	}
}