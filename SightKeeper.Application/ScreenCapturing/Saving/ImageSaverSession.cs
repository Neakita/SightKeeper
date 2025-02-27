using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public abstract class ImageSaverSession<TPixel> : IDisposable
{
	public abstract void CreateImage(ReadOnlySpan2D<TPixel> imageData);

	public abstract void Dispose();

	internal ImageSet Set { get; }

	protected ImageDataAccess ImageDataAccess { get; }

	protected ImageSaverSession(ImageSet set, ImageDataAccess imageDataAccess)
	{
		Set = set;
		ImageDataAccess = imageDataAccess;
	}
}