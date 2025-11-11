using Serilog;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

internal sealed class DistributedTrainDataExporter<TTag, TAsset>(TrainDataExporter<TTag, TAsset> inner, ILogger logger) : TrainDataExporter<TTag, TAsset> where TAsset : ReadOnlyAsset
{
	public string TrainDirectoryName { get; set; } = "train";
	public string ValidationDirectoryName { get; set; } = "validation";
	public string TestDirectoryName { get; set; } = "test";

	public AssetsDistributionRequest DistributionRequest { get; set; } = new()
	{
		TrainFraction = .90f,
		ValidationFraction = .05f,
		TestFraction = .05f
	};

	public async Task ExportAsync(string path, ReadOnlyDataSet<TTag, TAsset> data, CancellationToken cancellationToken)
	{
		var distribution = AssetsDistributor.DistributeAssets(data.Assets, DistributionRequest);
		logger.Debug("Exporting train data");
		await inner.ExportAsync(Path.Combine(path, TrainDirectoryName), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TrainAssets), cancellationToken);
		logger.Debug("Exporting validation data");
		await inner.ExportAsync(Path.Combine(path, ValidationDirectoryName), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.ValidationAssets), cancellationToken);
		logger.Debug("Exporting test data");
		await inner.ExportAsync(Path.Combine(path, TestDirectoryName), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TestAssets), cancellationToken);
	}
}