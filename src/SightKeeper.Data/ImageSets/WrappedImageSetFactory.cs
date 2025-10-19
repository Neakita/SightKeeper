using SightKeeper.Application;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal sealed class WrappedImageSetFactory(
	Factory<ImageSet> innerFactory,
	Wrapper<ImageSet> wrapper)
	: Factory<ImageSet>
{
	public ImageSet Create()
    {
        var innerSet = innerFactory.Create();
		return wrapper.Wrap(innerSet);
	}
}