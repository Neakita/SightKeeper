using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class WrappedImageSetFactory : ImageSetFactory<ImageSet>
{
	public WrappedImageSetFactory(ImageSetFactory<ImageSet> innerFactory, ImageSetWrapper wrapper)
	{
        _innerFactory = innerFactory;
		_setWrapper = wrapper;
	}

	public ImageSet CreateImageSet()
    {
        var innerSet = _innerFactory.CreateImageSet();
		return _setWrapper.Wrap(innerSet);
	}

    private readonly ImageSetFactory<ImageSet> _innerFactory;
	private readonly ImageSetWrapper _setWrapper;
}