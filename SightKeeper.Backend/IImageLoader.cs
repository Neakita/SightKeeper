using SightKeeper.DAL;

namespace SightKeeper.Backend;

public interface IImageLoader
{
	Image GetImageFromFile(string filePath);
}