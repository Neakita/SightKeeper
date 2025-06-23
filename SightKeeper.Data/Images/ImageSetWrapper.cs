using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

internal sealed class ImageSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public ImageSet Wrap(ImageSet set)
	{
		var decoratedSet = set
			// Tracking is locked because we don't want potential double saving when after modifying saving thread will immediately save and consider changes handled,
			// and then tracking decorator will send another notification.
			.WithTracking(changeListener)
			// Locking of domain rules can be relatively computationally heavy,
			// for example when removing images range every image should be checked if it is used by some asset,
			// so locking appears only after domain rules validated.
			.WithLocking(editingLock)
			// Images observing decorator can also be before domain rules,
			// but domain rules can often throw exceptions so placing observing decorator after domain rules will make stack trace a bit shorter.
			// Observer should be able to stream received images, so observable decorator should contain streamable decorator.
			.WithObservableImages()
			.WithDomainRules();
		return new StorableImageSet(decoratedSet);
	}
}