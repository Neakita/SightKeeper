using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Training.Images;

public interface ImagesExporter
{
	Task<IReadOnlyCollection<string>> Export(
		string targetDirectoryPath,
		Domain.Model.DataSet dataSet,
		CancellationToken cancellationToken = default);
	
	Task<IReadOnlyCollection<string>> Export(
		string targetDirectoryPath,
		IReadOnlyCollection<Asset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken = default);
	
	// TODO
	// IObservable<string> ExportAsync(string targetDirectoryPath, IReadOnlyCollection<Asset> assets, CancellationToken cancellationToken = default);
}