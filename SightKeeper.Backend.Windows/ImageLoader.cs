using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Backend.Windows;

public sealed class ImageLoader : IImageLoader
{
	public Image GetImageFromFile(string filePath)
	{
		System.Drawing.Image image = System.Drawing.Image.FromFile(filePath);
		if (image.Size.Width > ushort.MaxValue)
			throw new Exception($"The image's width resolution ({image.Size.Width}) exceeds the maximum allowed {ushort.MaxValue} pixels.");
		if (image.Size.Height > ushort.MaxValue)
			throw new Exception($"The image's height resolution ({image.Size.Height}) exceeds the maximum allowed {ushort.MaxValue} pixels.");
		return GDIImageToDomainImage(image);
	}

	private Image GDIImageToDomainImage(System.Drawing.Image image)
	{
		byte[]? bytes = (byte[]?) ImageConverter.ImageToBytes(image);
		if (bytes == null) throw new Exception("Failed to convert image to byte array.");
		return new Image(bytes, new Resolution((ushort) image.Size.Width, (ushort) image.Size.Height));
	}
}