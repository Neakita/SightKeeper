using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application.ScreenCapturing;

public interface WriteImageDataAccess
{
	void SaveImageData(Image image, ReadOnlySpan2D<Rgba32> data);
	void DeleteImageData(Image image);
}