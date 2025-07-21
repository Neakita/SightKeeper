using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data;

public interface ImageLookupperPopulator
{
	void AddImages(IEnumerable<StorableImage> images);
	void ClearImages();
}