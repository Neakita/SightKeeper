using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

internal sealed class ImageSetWrapper(ChangeListener changeListener, Lock editingLock, FileSystemDataAccess imagesDataAccess)
{
	public ImageSet Wrap(PackableImageSet packable)
	{
		return new StorableImageSet(packable, editingLock, changeListener, imagesDataAccess);
	}
}