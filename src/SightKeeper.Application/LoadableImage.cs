using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Application;

public interface LoadableImage
{
	Task<Image> LoadAsync(CancellationToken cancellationToken);
	Task<Image<TPixel>> LoadAsync<TPixel>(CancellationToken cancellationToken) where TPixel : unmanaged, IPixel<TPixel>;
}