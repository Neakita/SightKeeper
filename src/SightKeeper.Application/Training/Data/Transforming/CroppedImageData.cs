using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace SightKeeper.Application.Training.Data.Transforming;

internal sealed class CroppedImageData(ImageData innerData, LoadableImage innerLoadable, Rectangle cropRectangle) : ImageData, LoadableImage
{
	public Vector2<ushort> Size => new((ushort)cropRectangle.Width, (ushort)cropRectangle.Height);
	public DateTimeOffset CreationTimestamp => innerData.CreationTimestamp;

	public async Task<Image?> LoadAsync(CancellationToken cancellationToken)
	{
		var image = await innerLoadable.LoadAsync(cancellationToken);
		if (image == null)
			return null;
		Crop(image);
		return image;
	}

	public async Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>
	{
		var image = await innerLoadable.LoadAsync<TPixel>(cancellationToken);
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