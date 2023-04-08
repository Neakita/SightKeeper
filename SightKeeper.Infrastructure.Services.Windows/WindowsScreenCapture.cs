using System.Drawing;
using System.Drawing.Imaging;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Bitmap = System.Drawing.Bitmap;

namespace SightKeeper.Infrastructure.Services.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public WindowsScreenCapture(ScreenBoundsProvider screenBoundsProvider)
	{
		_screenBoundsProvider = screenBoundsProvider;
	}
	
	public byte[] Capture()
	{
		if (Resolution == null) throw new InvalidOperationException("Resolution is null");
		using Bitmap windowsBitmap = new(Resolution.Width, Resolution.Height);
		using Graphics graphics = Graphics.FromImage(windowsBitmap);

		Point point = new(
			_screenBoundsProvider.MainScreenCenter.X - Resolution.Width / 2 - XOffset, 
			_screenBoundsProvider.MainScreenCenter.Y - Resolution.Height / 2 - YOffset);

		graphics.CopyFromScreen(point, Point.Empty, new Size(Resolution.Width, Resolution.Height));
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		return stream.ToArray();
	}

	public bool IsEnabled { get; set; }
	public Game? Game { get; set; }
	public Resolution? Resolution { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
	
	private readonly ScreenBoundsProvider _screenBoundsProvider;
}
