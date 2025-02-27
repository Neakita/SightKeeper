using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaverSession<TPixel> : IDisposable
{
	public abstract void CreateScreenshot(ReadOnlySpan2D<TPixel> imageData);

	public abstract void Dispose();

	internal ImageSet Library { get; }

	protected ImageDataAccess ImageDataAccess { get; }

	protected ScreenshotsSaverSession(ImageSet library, ImageDataAccess imageDataAccess)
	{
		Library = library;
		ImageDataAccess = imageDataAccess;
	}
}