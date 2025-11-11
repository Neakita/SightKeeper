using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Application.Misc;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training;

internal sealed class ParallelImageExporter(ILogger logger, IObserver<Progress> progressObserver) : ImageExporter
{
	public async Task ExportImagesAsync(string directoryPath, IReadOnlyCollection<ImageData> images, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Debug).Begin("{ImagesCount} Images export", images.Count);
		Directory.CreateDirectory(directoryPath);
		var timeEstimator = new RemainingTimeEstimator(images.Count);
		var processedImages = 0;
		await Parallel.ForEachAsync(images.Index(), cancellationToken, async (tuple, bodyCancellationToken) =>
		{
			var (index, data) = tuple;
			var imageFileName = GetImageFileName(index);
			var imageFilePath = Path.Combine(directoryPath, imageFileName);
			await ExportImageAsync(imageFilePath, data, bodyCancellationToken);
			var incrementedProcessedImages = Interlocked.Increment(ref processedImages);
			ReportProgress(incrementedProcessedImages);
		});
		operation.Complete();

		void ReportProgress(int incrementedProcessedImages)
		{
			var remainingTime = timeEstimator.Estimate(incrementedProcessedImages);
			var progress = new Progress
			{
				Label = "Exporting images",
				Current = incrementedProcessedImages,
				Total = images.Count,
				EstimatedTimeOfArrival = DateTime.Now + remainingTime
			};
			progressObserver.OnNext(progress);
		}
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