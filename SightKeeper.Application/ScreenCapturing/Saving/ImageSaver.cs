using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface ImageSaver<TPixel>
{
	void SaveImage(DomainImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp);
}