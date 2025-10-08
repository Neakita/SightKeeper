using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

internal interface ImageSaver<TPixel>
{
	void SaveImage(ImageSet set, ReadOnlySpan2D<TPixel> imageData, DateTimeOffset creationTimestamp);
}