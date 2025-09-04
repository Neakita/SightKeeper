using SightKeeper.Domain.Images;

namespace SightKeeper.Data.ImageSets.Images;

public interface ImageWrapper
{
	ManagedImage Wrap(ManagedImage image);
}