using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public interface ImageLoader<TPixel>
{
	bool LoadImage(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken);
	Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken);
}