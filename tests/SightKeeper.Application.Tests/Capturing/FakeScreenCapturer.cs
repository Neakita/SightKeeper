using CommunityToolkit.HighPerformance;
using SightKeeper.Application.ScreenCapturing;
using SightKeeper.Domain;

namespace SightKeeper.Application.Tests.Capturing;

internal sealed class FakeScreenCapturer<TPixel> : ScreenCapturer<TPixel>
{
	public List<(Vector2<ushort> resolution, Vector2<ushort> offset)> CaptureCalls { get; } = new();

	public ReadOnlySpan2D<TPixel> Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		CaptureCalls.Add((resolution, offset));
		var array = new TPixel[resolution.X * resolution.Y];
		return array.AsSpan().AsSpan2D(resolution.Y, resolution.X);
	}
}