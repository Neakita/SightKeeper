using System.Drawing;

namespace SightKeeper.Backend.Windows;

public static class ImageConverter
{
	public static Image BytesToImage(byte[] bytes)
	{
		using MemoryStream stream = new(bytes);
		return new Bitmap(stream);
	}

	public static byte[] ImageToBytes(Image image)
	{
		using MemoryStream stream = new();
		image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
		return stream.ToArray();
	}
}
