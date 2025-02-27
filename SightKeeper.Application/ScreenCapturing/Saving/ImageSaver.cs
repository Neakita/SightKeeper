using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public abstract class ImageSaver<TPixel> : IDisposable
{
	public abstract Vector2<ushort> MaximumImageSize { get; set; }

	public abstract ImageSaverSession<TPixel> AcquireSession(ImageSet library);
	public abstract void ReleaseSession(ImageSaverSession<TPixel> session);

	public abstract void Dispose();
}