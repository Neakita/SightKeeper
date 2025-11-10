using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training;

internal interface ImageExporter
{
	Task ExportImagesAsync(string directoryPath, IReadOnlyCollection<ImageData> images, CancellationToken cancellationToken);
}