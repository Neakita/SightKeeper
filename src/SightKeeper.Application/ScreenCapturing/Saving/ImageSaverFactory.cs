namespace SightKeeper.Application.ScreenCapturing.Saving;

internal interface ImageSaverFactory<TPixel>
{
	ImageSaver<TPixel> CreateImageSaver();
}