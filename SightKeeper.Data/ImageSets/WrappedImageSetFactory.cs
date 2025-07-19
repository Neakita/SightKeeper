using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

public sealed class WrappedImageSetFactory : ImageSetFactory<StorableImageSet>, ImageSetFactory<ImageSet>
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

	ImageSet ImageSetFactory<ImageSet>.CreateImageSet()
	{
		return CreateImageSet();
	}

	private readonly ImageSetFactory<StorableImageSet> _innerFactory;
	private readonly ImageSetWrapper _setWrapper;
}