using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public interface ImageLoader<TPixel>
{
	Task<bool> LoadImageAsync(Image image, Memory<TPixel> target, CancellationToken cancellationToken);
}