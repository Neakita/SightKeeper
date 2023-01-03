using System.Drawing;
using System.Windows.Forms;
using SightKeeper.DAL.Domain.Common;
using Image = SightKeeper.DAL.Image;

namespace SightKeeper.Backend.Windows;

public sealed class Screenshoter : IScreenshoter
{
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	
	
	public Screenshoter(Resolution resolution)
	{
		if (Screen.AllScreens.Length > 1)
			throw new NotSupportedException("More than 1 screen is not supported for now.");
		Width = resolution.Width;
		Height = resolution.Height;
		_sourceX = (Screen.PrimaryScreen.Bounds.Width / 2) - (Width / 2);
		_sourceY = (Screen.PrimaryScreen.Bounds.Height / 2) - (Height / 2);
	}
	
	
	public Image MakeScreenshot()
	{
		Bitmap bitmap = new(Width, Height);
		using Graphics captureGraphic = Graphics.FromImage(bitmap);
		captureGraphic.CopyFromScreen(_sourceX, _sourceY, 0, 0, bitmap.Size);
		Image image = new(ImageToBytes(bitmap), new Resolution(Width, Height));
		return image;
	}


	private readonly int _sourceX;
	private readonly int _sourceY;
	
	public static byte[] ImageToBytes(Bitmap image)
	{
		using MemoryStream stream = new();
		image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
		return stream.ToArray();
	}
}
