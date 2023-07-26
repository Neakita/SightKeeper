using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Image = SightKeeper.Domain.Model.Common.Image;

namespace SightKeeper.Services.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}
	
	public Image Capture()
	{
		Guard.IsNotNull(Resolution);
		using Bitmap windowsBitmap = new(Resolution!.Width, Resolution.Height);
		using Graphics graphics = Graphics.FromImage(windowsBitmap);

		Point point = new(
			_screenBoundsProvider.MainScreenHorizontalCenter - Resolution.Width / 2 - XOffset, 
			_screenBoundsProvider.MainScreenVerticalCenter - Resolution.Height / 2 - YOffset);

		graphics.CopyFromScreen(point, Point.Empty, new Size(Resolution.Width, Resolution.Height));
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		return new Image(stream.ToArray());
	}

	public Task<Image> CaptureAsync(CancellationToken cancellationToken = default) =>
		Task.FromResult(Capture());

	public Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
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
