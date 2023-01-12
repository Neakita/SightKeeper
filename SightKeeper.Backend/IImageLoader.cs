using SightKeeper.Domain.Common;

namespace SightKeeper.Backend;

public interface IImageLoader
{
	Image GetImageFromFile(string filePath);
}