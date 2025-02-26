using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Screenshotting.Saving;

public abstract class ScreenshotsSaver<TPixel> : IDisposable
{
	public abstract Vector2<ushort> MaximumImageSize { get; set; }

	public abstract ScreenshotsSaverSession<TPixel> AcquireSession(ImageSet library);
	public abstract void ReleaseSession(ScreenshotsSaverSession<TPixel> session);

	public abstract void Dispose();
}