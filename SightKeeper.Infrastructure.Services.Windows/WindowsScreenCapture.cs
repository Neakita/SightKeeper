using System.Drawing;
using System.Drawing.Imaging;
using Avalonia.Media.Imaging;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Bitmap = System.Drawing.Bitmap;

namespace SightKeeper.Infrastructure.Services.Windows;

public sealed class WindowsScreenCapture : ScreenCapture
{
	public IBitmap Capture()
	{
		using Bitmap windowsBitmap = new(Width, Height);
		using Graphics graphics = Graphics.FromImage(windowsBitmap);
		graphics.CopyFromScreen(new Point(XOffset, YOffset), Point.Empty, new Size(Width, Height));
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		stream.Position = 0;
		Avalonia.Media.Imaging.Bitmap result = new(stream);
		return result;
	}

	public bool IsEnabled { get; set; }
	public Game? Game { get; set; }
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
}
