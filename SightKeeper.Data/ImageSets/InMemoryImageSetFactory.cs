using SightKeeper.Application.ImageSets.Creating;
using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.ImageSets;

public class InMemoryImageSetFactory(ImageWrapper imageWrapper) : ImageSetFactory<InMemoryImageSet>
{
    public InMemoryImageSet CreateImageSet()
    {
        return new InMemoryImageSet(imageWrapper);
    }
}