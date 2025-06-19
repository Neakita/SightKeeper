using SightKeeper.Application;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

public sealed class StorableImageSetFactory
{
	public StorableImageSetFactory(ChangeListener changeListener, [Tag(typeof(AppData))] Lock editingLock)
	{
		var imageDataAccess = new CompressedFileSystemDataAccess();
		imageDataAccess.DirectoryPath = Path.Combine(imageDataAccess.DirectoryPath, "Images");
		_wrapper = new ImageSetWrapper(changeListener, editingLock, imageDataAccess);
	}

	public ImageSet CreateImageSet()
	{
		var set = new PackableImageSet();
		return _wrapper.Wrap(set);
	}

	private readonly ImageSetWrapper _wrapper;
}