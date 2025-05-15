using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public interface ReadImageDataAccess
{
	Stream LoadImage(Image image);
}