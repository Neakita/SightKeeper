using SightKeeper.Domain.Images;

namespace SightKeeper.Data;

internal interface ImageLookupperPopulator
{
	void AddImages(IEnumerable<ManagedImage> images);
	void AddImage(ManagedImage image);
	void RemoveImage(ManagedImage image);
	void ClearImages();
}