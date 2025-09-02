using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.ScreenCapturing.Saving;

public interface ImageDataSaver<TPixel>
{
	void SaveData(ManagedImage image, ReadOnlySpan2D<TPixel> data);
}