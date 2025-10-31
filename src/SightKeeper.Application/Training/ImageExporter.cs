using Serilog;
using Serilog.Events;
using SerilogTimings.Extensions;
using SightKeeper.Domain;
using SightKeeper.Domain.Images;
using SixLabors.ImageSharp;

namespace SightKeeper.Application.Training;

internal sealed class ImageExporter(ILogger logger)
{
	public async Task ExportImageAsync(string filePath, ImageData data, CancellationToken cancellationToken)
	{
		using var operation = logger.OperationAt(LogEventLevel.Verbose).Begin("Image {image} export", data);
		var loadableImage = data.GetFirst<LoadableImage>();
		using var image = await loadableImage.LoadAsync(cancellationToken);
		await image.SaveAsync(filePath, cancellationToken);
		operation.Complete();
	}
}