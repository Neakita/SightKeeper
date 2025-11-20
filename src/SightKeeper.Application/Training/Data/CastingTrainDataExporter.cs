using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.Data;

internal sealed class CastingTrainDataExporter<TTag, TAsset>(TrainDataExporter<TTag, TAsset> inner) : TrainDataExporter<ReadOnlyTag, ReadOnlyAsset>
{
	public Task ExportAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyAsset> data, CancellationToken cancellationToken)
	{
		return inner.ExportAsync((ReadOnlyDataSet<TTag, TAsset>)data, cancellationToken);
	}
}