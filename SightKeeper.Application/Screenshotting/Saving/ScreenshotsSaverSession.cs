using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaverSession<TPixel> : IDisposable
{
	public abstract void CreateScreenshot(ReadOnlySpan2D<TPixel> imageData);

	public abstract void Dispose();

	internal ScreenshotsLibrary Library { get; }

	protected ScreenshotsDataAccess ScreenshotsDataAccess { get; }

	protected ScreenshotsSaverSession(ScreenshotsLibrary library, ScreenshotsDataAccess screenshotsDataAccess)
	{
		Library = library;
		ScreenshotsDataAccess = screenshotsDataAccess;
	}
}