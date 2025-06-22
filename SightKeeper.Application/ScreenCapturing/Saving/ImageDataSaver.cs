using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface ImageDataSaver<TPixel>
	where TPixel : unmanaged
{
	void SaveData(Image image, ReadOnlySpan2D<TPixel> data);
}