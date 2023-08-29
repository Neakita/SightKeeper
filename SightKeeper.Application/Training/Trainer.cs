using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class Trainer
{
	private const string DataDirectoryPath = "Data/Training/";
	private const string DataSetPath = DataDirectoryPath + "data.yaml";
	private const string RunsDirectoryPath = DataDirectoryPath + "Runs/";

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
		PrepareDataDirectory();
		await _imagesExporter.Export(DataDirectoryPath, dataSet, cancellationToken);
		await ExportDataSet(dataSet, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath));
		CLITrainerArguments arguments = new(DataSetPath, size, epochs);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastAsync();
		Log.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress, size);
	}

	public Task<Weights?> ResumeTrainingAsync(
		Weights weights,
		CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
	
	private readonly ImagesExporter _imagesExporter;
	private readonly DataSetConfigurationExporter _dataSetConfigurationExporter;
	private readonly WeightsDataAccess _weightsDataAccess;

	private void PrepareDataDirectory()
	{
		Directory.Delete(DataDirectoryPath, true);
		Directory.CreateDirectory(DataDirectoryPath);
	}

	private async Task ExportDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken)
	{
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(DataDirectoryPath), dataSet.ItemClasses);
		await _dataSetConfigurationExporter.Export(DataSetPath, dataSetParameters, cancellationToken);
	}
	
	private async Task<Weights> SaveWeights(Domain.Model.DataSet dataSet, TrainingProgress lastProgress, ModelSize size)
	{
		var runDirectory = Directory.GetDirectories(RunsDirectoryPath).Single();
		var modelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(modelPath, dataSet.Resolution);
		var bytes = await File.ReadAllBytesAsync(onnxModelPath, CancellationToken.None);
		return await _weightsDataAccess.CreateWeights(dataSet.WeightsLibrary, bytes, size, lastProgress.CurrentEpoch,
			lastProgress.BoundingLoss, lastProgress.ClassificationLoss, lastProgress.DeformationLoss, dataSet.Assets, CancellationToken.None);
	}
}