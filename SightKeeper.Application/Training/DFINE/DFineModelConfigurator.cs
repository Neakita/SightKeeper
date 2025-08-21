using SightKeeper.Domain;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineModelConfigurator(string repositoryDirectoryPath)
{
	public async Task AdjustModelConfigAsync(Vector2<ushort> inputSize, CancellationToken cancellationToken)
	{
		if (!File.Exists(OriginalConfigPath))
			File.Copy(ConfigPath, OriginalConfigPath);

		var configLines = await File.ReadAllLinesAsync(OriginalConfigPath, cancellationToken);
		
		configLines.ReplaceAt(7, "640, 640", $"{inputSize.Y}, {inputSize.X}");
		
		await File.WriteAllLinesAsync(ConfigPath, configLines, cancellationToken);
	}

	private string ConfigPath => Path.Combine(repositoryDirectoryPath, "configs", "dfine", "include", "dfine_hgnetv2.yml");
	private string OriginalConfigPath => Path.Combine(repositoryDirectoryPath, "configs", "dfine", "include", "original_dfine_hgnetv2.yml");
}