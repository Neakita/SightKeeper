using CommunityToolkit.HighPerformance;
using SightKeeper.Domain.Model;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public interface ScreenCapture
{
	ReadOnlySpan2D<Bgra32> Capture(Vector2<ushort> resolution, Vector2<ushort> offset);
}