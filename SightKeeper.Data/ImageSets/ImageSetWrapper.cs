using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets;

internal interface ImageSetWrapper
{
	ImageSet Wrap(ImageSet set);
}