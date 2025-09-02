using SightKeeper.Domain;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineConfigurator(string repositoryDirectoryPath, string dataSetDirectoryPath)
{
	public async Task ConfigureAsync(byte batchSize, Vector2<ushort> inputSize, byte tagsCount, DFineModel model, string outputDirectoryPath, CancellationToken cancellationToken)
	{
		await _dataLoaderConfigurator.AdjustDataLoaderConfigAsync(batchSize, inputSize, cancellationToken);
		await _detectionConfigurator.AdjustConfigAsync(tagsCount, cancellationToken);
		await _modelConfigurator.AdjustModelConfigAsync(inputSize, cancellationToken);
		await _sizedModelConfigurator.AdjustModelConfigAsync(model, batchSize, outputDirectoryPath, cancellationToken);
	}

	private readonly DFineDataLoaderConfigurator _dataLoaderConfigurator = new(repositoryDirectoryPath);
	private readonly DFineDetectionConfigurator _detectionConfigurator = new(repositoryDirectoryPath, dataSetDirectoryPath);
	private readonly DFineModelConfigurator _modelConfigurator = new(repositoryDirectoryPath);
	private readonly DFineSizedModelConfigurator _sizedModelConfigurator = new(repositoryDirectoryPath);
}