using CommunityToolkit.HighPerformance;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application;

public sealed class BinaryImageLoader<TPixel> : ImageLoader<TPixel> where TPixel : unmanaged
{
	public async Task<bool> LoadImageAsync(Image image, Memory<TPixel> target, CancellationToken cancellationToken)
	{
		var targetAsBytes = target.Cast<TPixel, byte>();
		await using var stream = image.OpenReadStream();
		if (stream == null)
			return false;
		using var operation = Logger.OperationAt(LogEventLevel.Verbose)
			.Begin("Loading image {image} with size {size}", image, image.Size);
		var totalBytesRead = 0;
		int lastBytesRead;
		do
		{
			if (cancellationToken.IsCancellationRequested)
				return false;
			lastBytesRead = await stream.ReadAsync(targetAsBytes[totalBytesRead..], CancellationToken.None);
			totalBytesRead += lastBytesRead;
		} while (lastBytesRead > 0);
		operation.Complete("totalBytesRead", totalBytesRead);
		return true;
	}

	private static readonly ILogger Logger = Log.ForContext<BinaryImageLoader<TPixel>>();
}