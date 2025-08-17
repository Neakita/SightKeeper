using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Training;

public interface ImageExporter
{
	Task ExportImageAsync(string filePath, Image image, CancellationToken cancellationToken);
}