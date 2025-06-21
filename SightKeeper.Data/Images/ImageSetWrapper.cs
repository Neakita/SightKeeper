using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class ImageSetWrapper(ChangeListener changeListener, Lock editingLock)
{
	public ImageSet Wrap(InMemoryImageSet inMemorySet)
	{
		return new StorableImageSet(inMemorySet, editingLock, changeListener);
	}
}