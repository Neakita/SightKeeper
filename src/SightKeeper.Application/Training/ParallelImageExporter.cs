using System.Diagnostics;
using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training;

internal sealed class ParallelImageExporter(ILogger logger) : ImageExporter
{
	public Task ExportImagesAsync(string directoryPath, IReadOnlyCollection<ImageData> images, CancellationToken cancellationToken)
	{
		var progressLock = new Lock();
		var etaComputer = new RemainingTimeEstimator(images.Count);
		var processedImages = 0;
		var lastLog = Stopwatch.StartNew();
		var lastInformationLog = Stopwatch.StartNew();
		return Parallel.ForEachAsync(images.Index(), cancellationToken, async (tuple, bodyCancellationToken) =>
		{
			var (index, data) = tuple;
			var imageFileName = GetImageFileName(index);
			var imageFilePath = Path.Combine(directoryPath, imageFileName);
			await ExportImageAsync(imageFilePath, data, bodyCancellationToken);
			Interlocked.Increment(ref processedImages);
			if (lastLog.Elapsed < TimeSpan.FromMilliseconds(100))
				return;
			lock (progressLock)
			{
				if (lastLog.Elapsed < TimeSpan.FromMilliseconds(100))
					return;
				lastLog.Restart();
				var logLevel = LogEventLevel.Verbose;
				if (lastInformationLog.Elapsed > TimeSpan.FromSeconds(2))
				{
					lastInformationLog.Restart();
					logLevel = LogEventLevel.Information;
				}
				var eta = etaComputer.Estimate(processedImages);
				logger.Write(logLevel, "Exporting images {imageIndex}/{totalImages} ETA: {ETA} ({CompletionTimestamp})",
					index, images.Count, eta, DateTime.Now + eta);
			}
		});
	}

	private async Task ExportImageAsync(string filePath, ImageData data, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Verbose).Begin("Image {image} export", data);
		var loadableImage = data.GetFirst<LoadableImage>();
		using var image = await loadableImage.LoadAsync(cancellationToken);
		await image.SaveAsync(filePath, cancellationToken);
		operation.Complete();
	}

	private static string GetImageFileName(int id)
	{
		return $"{id}.png";
	}
}