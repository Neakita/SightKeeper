using SightKeeper.Application.Misc;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal class InMemoryImageSetFactory(Wrapper<ManagedImage> imageWrapper) : Factory<InMemoryImageSet>
{
    public InMemoryImageSet Create()
    {
        return new InMemoryImageSet(imageWrapper);
    }
}