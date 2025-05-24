using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;

namespace SightKeeper.Application.ScreenCapturing;

public interface ScreenCapturer<TPixel>
{
	ReadOnlySpan2D<TPixel> Capture(Vector2<ushort> resolution, Vector2<ushort> offset);
}