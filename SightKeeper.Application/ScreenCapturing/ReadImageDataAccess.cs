namespace SightKeeper.Application.ScreenCapturing;

public interface ReadImageDataAccess
{
	Stream LoadImage(DomainImage image);
}