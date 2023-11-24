using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using Serilog.Events;
using SerilogTimings;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;

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
	
	public byte[] Capture()
	{
		Guard.IsNotNull(Resolution);
		var operation = Operation.At(LogEventLevel.Verbose).Begin("Screen capturing");
		using Bitmap windowsBitmap = new(Resolution.Value, Resolution.Value);
		using var graphics = Graphics.FromImage(windowsBitmap);

		Point point = new(
			_screenCenter.X - Resolution.Value / 2 - XOffset, 
			_screenCenter.Y - Resolution.Value / 2 - YOffset);

		graphics.CopyFromScreen(point, Point.Empty, new Size(Resolution.Value, Resolution.Value));
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		operation.Complete();
		return stream.ToArray();
	}

	public Task<byte[]> CaptureAsync(CancellationToken cancellationToken = default) =>
		Task.Run(Capture, cancellationToken);

	public Game? Game { get; set; }
	public ushort? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }

	private readonly Point _screenCenter;
}
