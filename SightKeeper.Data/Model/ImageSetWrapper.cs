using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model;

internal sealed class ImageSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public ImageSet Wrap(PackableImageSet packable)
	{
		return new StorableImageSet(packable, editingLock, changeListener);
	}
}