using CommunityToolkit.HighPerformance;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public interface WriteImageDataAccess
{
	void SaveImageData(DomainImage image, ReadOnlySpan2D<Rgba32> data);
	void DeleteImageData(DomainImage image);
}