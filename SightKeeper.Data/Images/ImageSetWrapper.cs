using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

internal interface ImageSetWrapper
{
	ImageSet Wrap(ImageSet set);
}