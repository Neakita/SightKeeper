using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using Serilog.Events;
using SerilogTimings;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Services.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenCenter = new Point(
			screenBoundsProvider.MainScreenHorizontalCenter,
			screenBoundsProvider.MainScreenVerticalCenter);
	}

	public WindowsScreenCapture(Point screenCenter)
	{
		_screenCenter = screenCenter;
	}
	
	public byte[] Capture(ushort resolution, Game? game)
	{
		var operation = Operation.At(LogEventLevel.Verbose).Begin("Screen capturing");
		using Bitmap windowsBitmap = new(resolution, resolution);
		using var graphics = Graphics.FromImage(windowsBitmap);
		var halfResolution = resolution / 2;
		Point position = new(_screenCenter.X - halfResolution, _screenCenter.Y - halfResolution);
		Size size = new(resolution, resolution);
		graphics.CopyFromScreen(position, Point.Empty, size);
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		operation.Complete();
		return stream.ToArray();
	}

	private readonly Point _screenCenter;
}
