namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface ImageSaverFactory<TPixel>
{
	ImageSaver<TPixel> CreateImageSaver();
}