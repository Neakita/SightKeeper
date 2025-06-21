using SightKeeper.Application;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

public sealed class StorableImageSetFactory
{
	public StorableImageSetFactory(ChangeListener changeListener, [Tag(typeof(AppData))] Lock editingLock)
	{
		var dataAccess = new CompressedFileSystemDataAccess();
		dataAccess.DirectoryPath = Path.Combine(dataAccess.DirectoryPath, "Images");
		_imageWrapper = new ImageWrapper(dataAccess);
		_setWrapper = new ImageSetWrapper(changeListener, editingLock);
	}

	public ImageSet CreateImageSet()
	{
		var set = new InMemoryImageSet(_imageWrapper);
		return _setWrapper.Wrap(set);
	}

	private readonly ImageWrapper _imageWrapper;
	private readonly ImageSetWrapper _setWrapper;
}