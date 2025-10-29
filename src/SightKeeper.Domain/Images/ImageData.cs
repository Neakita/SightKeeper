using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Domain.Images;

public interface ImageData
{
	Vector2<ushort> Size { get; }
	DateTimeOffset CreationTimestamp { get; }
	Task<Image?> LoadAsync(CancellationToken cancellationToken);
	Task<Image<TPixel>?> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>;
}