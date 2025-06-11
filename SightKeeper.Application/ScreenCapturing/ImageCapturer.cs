using SightKeeper.Domain;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing;

public interface ImageCapturer
{
	Vector2<ushort> ImageSize { get; set; }
	double? FrameRateLimit { get; set; }
	DomainImageSet? Set { get; set; }
	IObservable<DomainImageSet?> SetChanged { get; }
}