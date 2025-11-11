using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Misc;

public interface ImageLoader<TPixel>
{
	Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken);
}