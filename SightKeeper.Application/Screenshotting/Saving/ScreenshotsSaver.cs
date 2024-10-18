using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaver<TPixel>
	where TPixel : unmanaged
{
	public abstract void CreateScreenshot(
		ScreenshotsLibrary library,
		ReadOnlySpan2D<TPixel> imageData,
		DateTimeOffset creationDate);
}