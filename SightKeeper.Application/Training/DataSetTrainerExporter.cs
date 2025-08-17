using SightKeeper.Application.Training.Assets.Distribution;

namespace SightKeeper.Application.Training;

public interface DataSetTrainerExporter<in TDataSet>
{
	string DirectoryPath { get; set; }
	Task ExportAsync(TDataSet dataSet, AssetsDistributionRequest assetsDistributionRequest, CancellationToken cancellationToken);
}