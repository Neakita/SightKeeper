using SightKeeper.Application;
using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class StorableImageSetFactory : ImageSetFactory
{
	public StorableImageSetFactory(ChangeListener changeListener, [Tag(typeof(AppData))] Lock editingLock)
	{
		var dataAccess = new CompressedFileSystemDataAccess();
		dataAccess.DirectoryPath = Path.Combine(dataAccess.DirectoryPath, "Images");
		_imageWrapper = new StorableImageWrapper(dataAccess);
		_setWrapper = new StorableImageSetWrapper(changeListener, editingLock);
	}

	public ImageSet CreateImageSet()
	{
		var set = new InMemoryImageSet(_imageWrapper);
		return _setWrapper.Wrap(set);
	}

	private readonly StorableImageWrapper _imageWrapper;
	private readonly StorableImageSetWrapper _setWrapper;
}