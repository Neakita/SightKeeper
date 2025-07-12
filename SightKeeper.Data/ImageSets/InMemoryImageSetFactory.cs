using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal class InMemoryImageSetFactory(ImageWrapper imageWrapper) : ImageSetFactory
{
    public ImageSet CreateImageSet()
    {
        return new InMemoryImageSet(imageWrapper);
    }
}