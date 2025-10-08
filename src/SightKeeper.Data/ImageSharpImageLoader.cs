using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp.PixelFormats;

namespace SightKeeper.Data;

internal sealed class ImageSharpImageLoader<TPixel> : ImageLoader<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
	public async Task<bool> LoadImageAsync(ImageData imageData, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		using var operation = Logger.OperationAt(LogEventLevel.Verbose)
			.Begin("Loading image {image} with size {size}", imageData, imageData.Size);
		using var image = await imageData.LoadAsync<TPixel>(cancellationToken);
		if (image == null || cancellationToken.IsCancellationRequested)
			return false;
		image.CopyPixelDataTo(target.Span);
		operation.Complete();
		return true;
	}

	private static readonly ILogger Logger = Log.ForContext<ImageSharpImageLoader<TPixel>>();
}