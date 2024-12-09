using SightKeeper.Domain;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaver<TPixel> : IDisposable
{
	public abstract Vector2<ushort> MaximumImageSize { get; set; }

	public abstract ScreenshotsSaverSession<TPixel> AcquireSession(ScreenshotsLibrary library);
	public abstract void ReleaseSession(ScreenshotsSaverSession<TPixel> session);

	public abstract void Dispose();
}