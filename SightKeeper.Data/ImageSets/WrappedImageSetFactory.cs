using SightKeeper.Application.ImageSets.Creating;

namespace SightKeeper.Data.ImageSets;

internal sealed class WrappedImageSetFactory : ImageSetFactory<StorableImageSet>
{
	public WrappedImageSetFactory(ImageSetFactory<StorableImageSet> innerFactory, ImageSetWrapper wrapper)
	{
        _innerFactory = innerFactory;
		_setWrapper = wrapper;
	}

	public StorableImageSet CreateImageSet()
    {
        var innerSet = _innerFactory.CreateImageSet();
		return _setWrapper.Wrap(innerSet);
	}

    private readonly ImageSetFactory<StorableImageSet> _innerFactory;
	private readonly ImageSetWrapper _setWrapper;
}