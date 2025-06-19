using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public interface ImageSetFactory
{
	ImageSet CreateImageSet();
}