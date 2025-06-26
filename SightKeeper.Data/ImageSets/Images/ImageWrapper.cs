using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal interface ImageWrapper
{
	Image Wrap(InMemoryImage image);
}