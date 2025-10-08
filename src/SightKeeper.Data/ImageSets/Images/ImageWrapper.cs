using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

internal interface ImageWrapper
{
	ManagedImage Wrap(ManagedImage image);
}