using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Screenshotting;

public interface ScreenCapture<TPixel>
{
	ReadOnlySpan2D<TPixel> Capture(Vector2<ushort> resolution, Vector2<ushort> offset);
}