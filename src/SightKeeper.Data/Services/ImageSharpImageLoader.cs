using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data.Services;

internal sealed class ImageSharpImageLoader<TPixel>(ILogger logger) : ImageLoader<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
	public async Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Verbose)
			.Begin("Loading image {image} with size {size}", imageData, imageData.Size);
		var loadable = imageData.GetFirst<LoadableImage>();
		using var image = await loadable.LoadAsync<TPixel>(cancellationToken);
		image.CopyPixelDataTo(target.Span);
		operation.Complete();
		return true;
	}
}