using Serilog;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class DistributedTrainDataExporter<TTag, TAsset>(
	TrainDataExporter<TTag, TAsset> train,
	TrainDataExporter<TTag, TAsset> validation,
	TrainDataExporter<TTag, TAsset> test,
	ILogger logger)
	: TrainDataExporter<TTag, TAsset> where TAsset : ReadOnlyAsset
{
	public AssetsDistributionRequest DistributionRequest { get; set; } = new()
	{
		TrainFraction = .90f,
		ValidationFraction = .05f,
		TestFraction = .05f
	};

	public async Task ExportAsync(ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken)
	{
		var distribution = AssetsDistributor.DistributeAssets(data.Assets, DistributionRequest);
		logger.Debug("Exporting train data");
		await train.ExportAsync(new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TrainAssets), cancellationToken);
		logger.Debug("Exporting validation data");
		await validation.ExportAsync(new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.ValidationAssets), cancellationToken);
		logger.Debug("Exporting test data");
		await test.ExportAsync(new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TestAssets), cancellationToken);
	}
}