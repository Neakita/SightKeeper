using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data;

internal interface TrainDataExporter<in TTag, in TAsset>
{
	Task ExportAsync(string path, ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken);
}