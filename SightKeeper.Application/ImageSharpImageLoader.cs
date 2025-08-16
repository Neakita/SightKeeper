using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SightKeeper.Domain.Images.Image;

namespace SightKeeper.Application;

public sealed class ImageSharpImageLoader<TPixel> : ImageLoader<TPixel> where TPixel : unmanaged, IPixel<TPixel>
{
	public async Task<bool> LoadImageAsync(Image image, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		await using var stream = image.OpenReadStream();
		if (stream == null || cancellationToken.IsCancellationRequested)
			return false;

		using var operation = Logger.OperationAt(LogEventLevel.Verbose)
			.Begin("Loading image {image} with size {size}", image, image.Size);

		Image<TPixel> imageData;
		try
		{
			imageData = await SixLabors.ImageSharp.Image.LoadAsync<TPixel>(stream, CancellationToken.None);
		}
		catch (UnknownImageFormatException exception)
		{
			Logger.Warning(
				exception,
				"An exception was thrown when trying to load an image {image}. " +
				"The image was probably corrupted when the application unexpectedly shut down.",
				image);
			operation.SetException(exception);
			return false;
		}

		try
		{
			if (cancellationToken.IsCancellationRequested)
				return false;
			imageData.CopyPixelDataTo(target.Span);
			operation.Complete("Length", target.Span.Length);
			return true;
		}
		finally
		{
			imageData.Dispose();
		}
	}

	private static readonly ILogger Logger = Log.ForContext<ImageSharpImageLoader<TPixel>>();
}