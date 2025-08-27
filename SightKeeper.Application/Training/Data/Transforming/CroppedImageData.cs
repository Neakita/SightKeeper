using SightKeeper.Domain;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class CroppedImageData(ImageData inner, Rectangle cropRectangle) : ImageData
{
	public Vector2<ushort> Size => new((ushort)cropRectangle.Width, (ushort)cropRectangle.Height);
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;
	public Image Image
	{
		get
		{
			var image = inner.Image;
			try
			{
				image.Mutate(context => context.Crop(cropRectangle));
			}
			catch
			{
				image.Dispose();
				throw;
			}
			return image;
		}
	}
}