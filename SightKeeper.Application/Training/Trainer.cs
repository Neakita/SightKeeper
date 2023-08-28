using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class Trainer
{
	public Trainer(ImagesExporter imagesExporter, DataSetConfigurationExporter dataSetConfigurationExporter, WeightsDataAccess weightsDataAccess)
	{
		_imagesExporter = imagesExporter;
		_dataSetConfigurationExporter = dataSetConfigurationExporter;
		_weightsDataAccess = weightsDataAccess;
	}
	
	public async Task<Weights?> TrainFromScratchAsync(
		Domain.Model.DataSet dataSet,
		ModelSize size,
		ushort epochs,
		CancellationToken cancellationToken = default)
	{
		const string dataPath = "Data/Training/";
		Directory.Delete(dataPath, true);
		Directory.CreateDirectory(dataPath);
		const string dataSetPath = dataPath + "data.yaml";
		await _imagesExporter.Export(dataPath, dataSet, cancellationToken);
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(dataPath), dataSet.ItemClasses);
		await _dataSetConfigurationExporter.Export(dataSetPath, dataSetParameters, cancellationToken);
		var runsDirectory = Path.GetFullPath(Path.Combine(dataPath, "Runs"));
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(runsDirectory);
		CLITrainerArguments arguments = new(dataSetPath, size, epochs);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString());
		TrainerParser.Parse(outputStream.WhereNotNull(), out var outputDirectoryName, out var trainingProgress);
		trainingProgress.Subscribe(progress => Log.Debug("Training progress: {TrainingProgress}", progress), () => Log.Debug("Output completed"));
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastAsync();
		Log.Debug("Last progress: {Progress}", lastProgress);
		var runDirectory = Directory.GetDirectories(runsDirectory).Single();
		var modelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(modelPath, dataSet.Resolution);
		var bytes = await File.ReadAllBytesAsync(onnxModelPath, cancellationToken);
		return await _weightsDataAccess.CreateWeights(dataSet.WeightsLibrary, bytes, size, lastProgress.CurrentEpoch,
			lastProgress.BoundingLoss, lastProgress.ClassificationLoss, lastProgress.DeformationLoss, dataSet.Assets, cancellationToken);
	}

	public Task<Weights?> ResumeTrainingAsync(
		Domain.Model.DataSet dataSet,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	
	private readonly ImagesExporter _imagesExporter;
	private readonly DataSetConfigurationExporter _dataSetConfigurationExporter;
	private readonly WeightsDataAccess _weightsDataAccess;
}