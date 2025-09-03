using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data;

public interface TrainDataExporter<in TAsset>
{
	Task ExportAsync(string path, ReadOnlyDataSet<TAsset> data, CancellationToken cancellationToken);
}