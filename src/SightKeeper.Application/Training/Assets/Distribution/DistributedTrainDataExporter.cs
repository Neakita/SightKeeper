using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Training.Assets.Distribution;

public sealed class DistributedTrainDataExporter<TTag, TAsset>(TrainDataExporter<TTag, TAsset> inner) : TrainDataExporter<TTag, TAsset> where TAsset : ReadOnlyAsset
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
		await inner.ExportAsync(Path.Combine(path, "train"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TrainAssets), cancellationToken);
		await inner.ExportAsync(Path.Combine(path, "validation"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.ValidationAssets), cancellationToken);
		await inner.ExportAsync(Path.Combine(path, "test"), new ReadOnlyDataSetValue<TTag, TAsset>(data.Tags, distribution.TestAssets), cancellationToken);
	}
}