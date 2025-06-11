using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ImageSets.Editing;

public interface ExistingImageSetData : ImageSetData
{
	DomainImageSet Set { get; }
}