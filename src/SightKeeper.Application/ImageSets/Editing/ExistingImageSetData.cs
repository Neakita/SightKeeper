using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public interface ExistingImageSetData : ImageSetData
{
	ImageSet Set { get; }
}