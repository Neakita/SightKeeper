using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface IImageLoader
{
	Image GetImageFromFile(string filePath);
}