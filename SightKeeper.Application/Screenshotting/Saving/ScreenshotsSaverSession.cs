using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaverSession<TPixel> : IDisposable
	where TPixel : unmanaged, IPixel<TPixel>
{
	public abstract void CreateScreenshot(ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationDate);

	public abstract void Dispose();

	internal ScreenshotsLibrary Library { get; }

	protected ScreenshotsDataAccess ScreenshotsDataAccess { get; }

	protected ScreenshotsSaverSession(ScreenshotsLibrary library, ScreenshotsDataAccess screenshotsDataAccess)
	{
		Library = library;
		ScreenshotsDataAccess = screenshotsDataAccess;
	}
}