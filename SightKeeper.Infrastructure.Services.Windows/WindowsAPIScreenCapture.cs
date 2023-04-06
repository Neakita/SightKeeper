using System.Drawing.Imaging;
using Avalonia.Media.Imaging;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Image = System.Drawing.Image;

namespace SightKeeper.Infrastructure.Services.Windows;

/*public sealed class WindowsAPIScreenCapture : ScreenCapture
{
	public IBitmap Capture()
	{
		throw new NotImplementedException();
		Image windowsBitmap = WindowsAPIScreenCaptureHelper.CaptureScreen();
		using MemoryStream stream = new();
		windowsBitmap.Save(stream, ImageFormat.Bmp);
		stream.Position = 0;
		Bitmap result = new(stream);
		return result;
	}

	public bool IsEnabled { get; set; }
	public Game? Game { get; set; }
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	public ushort XOffset { get; set; }
	public ushort YOffset { get; set; }
}*/