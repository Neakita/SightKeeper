using SightKeeper.Domain.Images;

namespace SightKeeper.Avalonia.ImageSets.Card;

public interface ImageSetCardDataContextFactory
{
	ImageSetCardDataContext CreateImageSetCardDataContext(ImageSet imageSet);
}