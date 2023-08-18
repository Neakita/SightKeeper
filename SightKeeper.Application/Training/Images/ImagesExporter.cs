namespace SightKeeper.Application.Training.Images;

public interface ImagesExporter<TDataSet> where TDataSet : Domain.Model.DataSet
{
	public Task<IReadOnlyCollection<string>> ExportAsync(string targetDirectoryPath, TDataSet model, CancellationToken cancellationToken = default);
}