namespace SightKeeper.Application.Training.Data.Transforming;

public sealed class TransformingTrainDataExporter<TAsset>(TrainDataExporter<TAsset> inner, TrainDataTransformer<TAsset> transformer) : TrainDataExporter<TAsset>
{
	public Task ExportAsync(string path, TrainData<TAsset> data, CancellationToken cancellationToken)
	{
		var transformedData = transformer.Transform(data);
		return inner.ExportAsync(path, transformedData, cancellationToken);
	}
}