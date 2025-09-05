using SightKeeper.Domain.DataSets;

namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class TransformingTrainDataExporter<TTag, TAsset>(TrainDataExporter<TTag, TAsset> inner, TrainDataTransformer<TTag, TAsset> transformer) : TrainDataExporter<TTag, TAsset>
{
	public Task ExportAsync(string path, ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken)
	{
		var transformedData = transformer.Transform(data);
		return inner.ExportAsync(path, transformedData, cancellationToken);
	}
}