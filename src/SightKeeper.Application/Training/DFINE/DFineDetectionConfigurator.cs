using Serilog;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineDetectionConfigurator(ILogger logger, string repositoryDirectoryPath, string dataSetDirectoryPath)
{
	public async Task AdjustConfigAsync(byte tagsCount, CancellationToken cancellationToken)
	{
		logger.Information("Adjusting detection config");
		if (!File.Exists(OriginalDetectionConfigPath))
			File.Copy(DetectionConfigPath, OriginalDetectionConfigPath);

		var configLines = await File.ReadAllLinesAsync(OriginalDetectionConfigPath, cancellationToken);

		// including background
		tagsCount++;

		configLines.ReplaceAt(6, "777", tagsCount.ToString());
		configLines.ReplaceAt(13, "/data/yourdataset/train", TrainImagesDirectoryPath);
		configLines.ReplaceAt(14, "/data/yourdataset/train/train.json", TrainDataSetFilePath);
		configLines.ReplaceAt(30, "/data/yourdataset/val", ValidationImagesDirectoryPath);
		configLines.ReplaceAt(31, "/data/yourdataset/val/val.json", ValidationDataSetFilePath);

		await File.WriteAllLinesAsync(DetectionConfigPath, configLines, cancellationToken);
	}

	private string ConfigsDirectoryPath => Path.Combine(repositoryDirectoryPath, "configs");
	private string DetectionConfigPath => Path.Combine(ConfigsDirectoryPath, "dataset", "custom_detection.yml");
	private string OriginalDetectionConfigPath => Path.Combine(ConfigsDirectoryPath, "dataset", "original_custom_detection.yml");
	private string TrainImagesDirectoryPath => Path.Combine(dataSetDirectoryPath, "train", "images");
	private string TrainDataSetFilePath => Path.Combine(dataSetDirectoryPath, "train", "data.json");
	private string ValidationImagesDirectoryPath => Path.Combine(dataSetDirectoryPath, "validation", "images");
	private string ValidationDataSetFilePath => Path.Combine(dataSetDirectoryPath, "validation", "data.json");
}