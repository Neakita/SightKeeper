using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class WrappedImageSetFactory : ImageSetFactory
{
	public WrappedImageSetFactory(ImageSetFactory innerFactory, ImageSetWrapper wrapper)
	{
        _innerFactory = innerFactory;
		_setWrapper = wrapper;
	}

	public ImageSet CreateImageSet()
    {
        var innerSet = _innerFactory.CreateImageSet();
		return _setWrapper.Wrap(innerSet);
	}

    private readonly ImageSetFactory _innerFactory;
	private readonly ImageSetWrapper _setWrapper;
}