using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public interface ImageCapturer
{
	Vector2<ushort> ImageSize { get; set; }
	double? FrameRateLimit { get; set; }
	ImageSet? Set { get; set; }
	IObservable<ImageSet?> SetChanged { get; }
}