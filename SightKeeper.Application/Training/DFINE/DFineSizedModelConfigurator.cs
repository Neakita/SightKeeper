using System.Globalization;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineSizedModelConfigurator(string repositoryDirectoryPath)
{
	public async Task AdjustModelConfigAsync(DFineModel model, byte batchSize, string outputDirectoryPath, CancellationToken cancellationToken)
	{
		var configPath = Path.Combine(ModelConfigsDirectoryPath, model.ConfigName);
		var originalConfigName = $"original_{model.ConfigName}";
		var originalConfigPath = Path.Combine(ModelConfigsDirectoryPath, originalConfigName);
		if (!File.Exists(originalConfigPath))
			File.Copy(configPath, originalConfigPath);
		await ReadDefaultBatchSizeAsync(cancellationToken);
		await AdjustModelConfigAsync(originalConfigPath, configPath, batchSize, outputDirectoryPath, cancellationToken);
	}

	private string ConfigsDirectoryPath => Path.Combine(repositoryDirectoryPath, "configs");
	private string ModelConfigsDirectoryPath => Path.Combine(ConfigsDirectoryPath, "dfine", "custom");
	private string OriginalDataLoaderConfigPath => Path.Combine(ConfigsDirectoryPath, "dfine", "include", "original_dataloader.yml");
	private static readonly Regex NumberRegex = new(@"\d+");
	private ushort _defaultOriginalBatchSize;

	private async Task ReadDefaultBatchSizeAsync(CancellationToken cancellationToken)
	{
		var configLines = await File.ReadAllLinesAsync(OriginalDataLoaderConfigPath, cancellationToken);
		var batchSizeLine = configLines[26];
		var batchSizeString = NumberRegex.Match(batchSizeLine).Value;
		var batchSize = ushort.Parse(batchSizeString);
		_defaultOriginalBatchSize = batchSize;
	}

	private async Task AdjustModelConfigAsync(string originalConfigPath, string configPath, byte batchSize, string outputDirectoryPath, CancellationToken cancellationToken)
	{
		var configLines = await File.ReadAllLinesAsync(originalConfigPath, cancellationToken);

		Guard.IsTrue(configLines[8].StartsWith("output_dir:"));
		configLines[8] = $"output_dir: {outputDirectoryPath}";

		var batchSizeLineRegex = new Regex(@"^\s*total_batch_size:\s*\d+$");
		var numberRegex = new Regex(@"\d+");

		var originalBatchSize = configLines.Where(line => batchSizeLineRegex.IsMatch(line))
			.Select(line => numberRegex.Match(line).Value)
			.Select(ushort.Parse)
			.FirstOrDefault(_defaultOriginalBatchSize);

		var decimalNumberRegex = new Regex(@"\d+\.\d+");

		var lrLineRegex = new Regex(@"^\s*lr:\s*\d+\.\d+$");
		var lrLineIndexes = configLines
			.Index()
			.Where(tuple => lrLineRegex.IsMatch(tuple.Item))
			.Select(tuple => tuple.Index);
		foreach (var lr1LineIndex in lrLineIndexes)
		{
			var originalLrLine = configLines[lr1LineIndex];
			var originalLrValueString = decimalNumberRegex.Match(originalLrLine).Value;
			var originalLrValue = decimal.Parse(originalLrValueString, CultureInfo.InvariantCulture);
			var originalLrPerBatch = originalLrValue / originalBatchSize;
			var modifiedLrValue = originalLrPerBatch * batchSize;
			var modifiedLrValueString = modifiedLrValue.ToString(CultureInfo.InvariantCulture);
			var modifiedLrLine = decimalNumberRegex.Replace(originalLrLine, modifiedLrValueString);
			configLines[lr1LineIndex] = modifiedLrLine;
		}

		var batchSizeLineIndexes = configLines
			.Index()
			.Where(tuple => batchSizeLineRegex.IsMatch(tuple.Item))
			.Select(tuple => tuple.Index);

		var batchSizeString = batchSize.ToString();
		foreach (var index in batchSizeLineIndexes)
		{
			var originalLine = configLines[index];
			var modifiedLine = numberRegex.Replace(originalLine, batchSizeString);
			configLines[index] = modifiedLine;
		}
		
		await File.WriteAllLinesAsync(configPath, configLines, cancellationToken);
	}
}