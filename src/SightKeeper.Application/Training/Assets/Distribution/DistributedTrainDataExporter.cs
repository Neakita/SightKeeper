using Serilog;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class DistributedTrainDataExporter<TTag, TAsset>(TrainDataExporter<TTag, TAsset> inner, ILogger logger) : TrainDataExporter<TTag, TAsset> where TAsset : ReadOnlyAsset
{
	public AssetsDistributionRequest DistributionRequest { get; set; } = new()
	{
		TrainFraction = .90f,
		ValidationFraction = .05f,
		TestFraction = .05f
	};

	public async Task ExportAsync(string path, ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken)
	{
		var distribution = AssetsDistributor.DistributeAssets(data.Assets, DistributionRequest);
		logger.Information("Exporting train data");
		await inner.ExportAsync(Path.Combine(path, "train"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TrainAssets), cancellationToken);
		logger.Information("Exporting validation data");
		await inner.ExportAsync(Path.Combine(path, "validation"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.ValidationAssets), cancellationToken);
		logger.Information("Exporting test data");
		await inner.ExportAsync(Path.Combine(path, "test"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TestAssets), cancellationToken);
	}
}