using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data;

internal interface ImageLookupperPopulator
{
	void AddImages(IEnumerable<StorableImage> images);
	void ClearImages();
}