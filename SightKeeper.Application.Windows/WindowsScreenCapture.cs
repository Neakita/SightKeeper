using System.Drawing;
using System.Drawing.Imaging;
using Serilog.Events;
using SerilogTimings;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}
	
	public Stream Capture(Vector2<ushort> resolution, Game? game)
	{
		var screenCenter = _screenBoundsProvider.MainScreenCenter;
		var operation = Operation.At(LogEventLevel.Verbose).Begin("Screen capturing");
		using Bitmap windowsBitmap = new(resolution.X, resolution.Y);
		using var graphics = Graphics.FromImage(windowsBitmap);
		var halfResolution = resolution / 2;
		Point position = new(screenCenter.X - halfResolution.X, screenCenter.Y - halfResolution.Y);
		Size size = new(resolution.X, resolution.Y);
		graphics.CopyFromScreen(position, Point.Empty, size);
		MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		operation.Complete();
		return stream;
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
}
