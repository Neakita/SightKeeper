using CommunityToolkit.HighPerformance;
using SightKeeper.Domain;

namespace SightKeeper.Application.ScreenCapturing;

public sealed class SustainableScreenCapturer<TPixel, TScreenCapturer> : ScreenCapturer<TPixel>
	where TScreenCapturer : ScreenCapturer<TPixel>, IDisposable, new()
{
	public ReadOnlySpan2D<TPixel> Capture(Vector2<ushort> resolution, Vector2<ushort> offset)
	{
		try
		{
			return _screenCapturer.Capture(resolution, offset);
		}
		catch
		{
			_screenCapturer.Dispose();
			_screenCapturer = new TScreenCapturer();
			throw;
		}
	}

	private TScreenCapturer _screenCapturer = new();
}