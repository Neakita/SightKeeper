using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

public interface ImageSetWrapper
{
	ImageSet Wrap(ImageSet set);
}