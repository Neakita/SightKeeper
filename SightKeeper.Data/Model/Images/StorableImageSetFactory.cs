using SightKeeper.Application;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.Images;

public sealed class StorableImageSetFactory
{
	public StorableImageSetFactory(ChangeListener changeListener, [Tag(typeof(AppData))] Lock editingLock)
	{
		_wrapper = new ImageSetWrapper(changeListener, editingLock);
	}

	public ImageSet CreateImageSet()
	{
		var set = new PackableImageSet();
		return _wrapper.Wrap(set);
	}

	private readonly ImageSetWrapper _wrapper;
}