using SightKeeper.Domain;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineDataLoaderConfigurator(string repositoryPath)
{
	public async Task AdjustDataLoaderConfigAsync(byte batchSize, Vector2<ushort> inputSize, CancellationToken cancellationToken)
	{
		if (!File.Exists(OriginalDataLoaderConfigPath))
			File.Copy(DataLoaderConfigPath, OriginalDataLoaderConfigPath);

		var configLines = (await File.ReadAllLinesAsync(OriginalDataLoaderConfigPath, cancellationToken)).ToList();
		
		var baseSize = inputSize.Y; // or it could be Math.Min(inputSize.X, inputSize.Y), not really sure
		configLines.ReplaceAt(21, "640", baseSize.ToString());
		configLines.ReplaceAt(26, "32", batchSize.ToString());
		configLines.ReplaceAt(37, "64", batchSize.ToString());
		configLines.RemoveAt(34);
		configLines.RemoveRange(5, 7);
		
		await File.WriteAllLinesAsync(DataLoaderConfigPath, configLines, cancellationToken);
	}

	private string DataLoaderConfigPath => Path.Combine(repositoryPath, "configs", "dfine", "include", "dataloader.yml");
	private string OriginalDataLoaderConfigPath => Path.Combine(repositoryPath, "configs", "dfine", "include", "original_dataloader.yml");
}