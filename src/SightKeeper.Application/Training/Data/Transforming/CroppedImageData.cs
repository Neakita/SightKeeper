using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class CroppedImageData(ImageData inner, Rectangle cropRectangle) : ImageData
{
	public Vector2<ushort> Size => new((ushort)cropRectangle.Width, (ushort)cropRectangle.Height);
	public DateTimeOffset CreationTimestamp => inner.CreationTimestamp;

	public Image? Load(CancellationToken cancellationToken)
	{
		var image = inner.Load(cancellationToken);
		if (image == null)
			return null;
		Crop(image);
		return image;
	}

	public Image<TPixel>? Load<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		var image = inner.Load<TPixel>(cancellationToken);
		if (image == null)
			return null;
		Crop(image);
		return image;
	}

	public async Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		var image = await inner.LoadAsync(cancellationToken);
		if (image == null)
			return null;
		Crop(image);
		return image;
	}

	public async Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		var image = await inner.LoadAsync<TPixel>(cancellationToken);
		if (image == null)
			return null;
		Crop(image);
		return image;
	}

	private void Crop(Image image)
	{
		image.Mutate(context => context.Crop(cropRectangle));
	}
}