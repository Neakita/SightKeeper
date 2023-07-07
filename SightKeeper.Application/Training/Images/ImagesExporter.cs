using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Training.Images;

public interface ImagesExporter<TModel> where TModel : Model
{
	public Task<IReadOnlyCollection<string>> ExportAsync(string targetDirectoryPath, TModel model, CancellationToken cancellationToken = default);
}