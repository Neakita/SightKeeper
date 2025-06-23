using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Images;

internal interface ImageWrapper
{
	Image Wrap(InMemoryImage image);
}