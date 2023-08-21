using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Training.Images;

public interface ImagesExporter<TAsset> where TAsset : Asset
{
	Task<IReadOnlyCollection<string>> ExportAsync(
		string targetDirectoryPath,
		DataSet<TAsset> dataSet,
		CancellationToken cancellationToken = default);
	
	Task<IReadOnlyCollection<string>> ExportAsync(
		string targetDirectoryPath,
		IReadOnlyCollection<TAsset> assets,
		IReadOnlyCollection<ItemClass> itemClasses,
		CancellationToken cancellationToken = default);
	
	// TODO
	// IObservable<string> ExportAsync(string targetDirectoryPath, IReadOnlyCollection<Asset> assets, CancellationToken cancellationToken = default);
}