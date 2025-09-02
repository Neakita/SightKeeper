namespace SightKeeper.Application.Training.Data;

public interface TrainDataExporter<in TAsset>
{
	Task ExportAsync(string path, TrainData<TAsset> data, CancellationToken cancellationToken);
}