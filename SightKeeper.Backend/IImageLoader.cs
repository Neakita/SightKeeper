using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Backend;

public interface IImageLoader
{
	Image GetImageFromFile(string filePath);
}