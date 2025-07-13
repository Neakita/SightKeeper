using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.ImageSets;

internal class InMemoryImageSetFactory(ImageWrapper imageWrapper) : ImageSetFactory<InMemoryImageSet>
{
    public InMemoryImageSet CreateImageSet()
    {
        return new InMemoryImageSet(imageWrapper);
    }
}