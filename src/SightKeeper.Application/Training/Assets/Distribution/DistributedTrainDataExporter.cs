using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

public sealed class DistributedTrainDataExporter<TAsset>(TrainDataExporter<TAsset> inner) : TrainDataExporter<TAsset> where TAsset : ReadOnlyAsset
{
	public AssetsDistributionRequest DistributionRequest { get; set; } = new()
	{
		TrainFraction = .90f,
		ValidationFraction = .05f,
		TestFraction = .05f
	};

	public async Task ExportAsync(string path, TrainData<TAsset> data, CancellationToken cancellationToken)
	{
		var distribution = AssetsDistributor.DistributeAssets(data.Assets, DistributionRequest);
		await inner.ExportAsync(Path.Combine(path, "train"), new TrainDataValue<TAsset>(data.Tags, distribution.TrainAssets), cancellationToken);
		await inner.ExportAsync(Path.Combine(path, "validation"), new TrainDataValue<TAsset>(data.Tags, distribution.ValidationAssets), cancellationToken);
		await inner.ExportAsync(Path.Combine(path, "test"), new TrainDataValue<TAsset>(data.Tags, distribution.TestAssets), cancellationToken);
	}
}