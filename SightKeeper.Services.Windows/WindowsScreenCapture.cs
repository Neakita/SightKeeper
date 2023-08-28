using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Services.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}
	
	public byte[] Capture()
	{
		Guard.IsNotNull(Resolution);
		using Bitmap windowsBitmap = new(Resolution.Value, Resolution.Value);
		using var graphics = Graphics.FromImage(windowsBitmap);

		Point point = new(
			_screenBoundsProvider.MainScreenHorizontalCenter - Resolution.Value / 2 - XOffset, 
			_screenBoundsProvider.MainScreenVerticalCenter - Resolution.Value / 2 - YOffset);

		graphics.CopyFromScreen(point, Point.Empty, new Size(Resolution.Value, Resolution.Value));
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		return stream.ToArray();
	}

	public Task<byte[]> CaptureAsync(CancellationToken cancellationToken = default) =>
		Task.Run(Capture, cancellationToken);

	public Game? Game { get; set; }
	public ushort? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }

	public bool CanCapture
	{
		get
		{
			if (Game == null) return true;
			Process? process = Process.GetProcessesByName(Game.ProcessName)
				.FirstOrDefault(process => process.MainWindowHandle > 0);
			if (process == null) return false;
			return User32.GetForegroundWindow() == process.MainWindowHandle;
		}
	}

	private readonly ScreenBoundsProvider _screenBoundsProvider;
}
