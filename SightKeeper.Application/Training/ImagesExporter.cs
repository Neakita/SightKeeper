using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Application.Training;

public interface ImagesExporter<TModel> where TModel : Model
{
	public IReadOnlyCollection<string> Export(string targetDirectoryPath, TModel model);
}