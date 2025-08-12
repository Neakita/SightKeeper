using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Creating;

public interface ImageSetFactory<out TImageSet> where TImageSet : ImageSet
{
	TImageSet CreateImageSet();
}