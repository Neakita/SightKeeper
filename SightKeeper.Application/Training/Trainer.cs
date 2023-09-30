using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class Trainer
{
	private static readonly string DataDirectoryPath = Path.Combine("Data", "Training");
	private static readonly string DataSetPath = Path.Combine(DataDirectoryPath, "data.yaml");
	private static readonly string RunsDirectoryPath = Path.Combine(DataDirectoryPath, "Runs");
	private static readonly string WeightsToResumeTrainingOnPath = Path.Combine(DataDirectoryPath, "weights.pt");

	public bool AMP { get; set; } = true;

	public Trainer(ImagesExporter imagesExporter, DataSetConfigurationExporter dataSetConfigurationExporter, WeightsDataAccess weightsDataAccess, ILogger logger)
	{
		_imagesExporter = imagesExporter;
		_dataSetConfigurationExporter = dataSetConfigurationExporter;
		_weightsDataAccess = weightsDataAccess;
		_logger = logger;
	}
	
	public async Task<Weights?> TrainFromScratchAsync(
		Domain.Model.DataSet dataSet,
		ModelSize size,
		uint epochs,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		await _imagesExporter.Export(DataDirectoryPath, dataSet, cancellationToken);
		await ExportDataSet(dataSet, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), _logger);
		CLITrainerArguments arguments = new(DataSetPath, size, epochs, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), _logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			_logger.Information("No training progress found");
			return null;
		}
		_logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress, size, CancellationToken.None); // TODO ability to abort saving
	}

	public async Task<Weights?> ResumeTrainingAsync(
		Weights weights,
		uint epochs,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		var dataSet = weights.Library.DataSet;
		await _imagesExporter.Export(DataDirectoryPath, dataSet, cancellationToken);
		await ExportDataSet(dataSet, cancellationToken);
		await ExportWeights(weights, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), _logger);
		CLITrainerArguments arguments = new(DataSetPath, Path.GetFullPath(WeightsToResumeTrainingOnPath), epochs, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), _logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			_logger.Information("No training progress found");
			return null;
		}
		_logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress, weights.Size, CancellationToken.None); // TODO ability to abort saving
	}
	
	private readonly ImagesExporter _imagesExporter;
	private readonly DataSetConfigurationExporter _dataSetConfigurationExporter;
	private readonly WeightsDataAccess _weightsDataAccess;
	private readonly ILogger _logger;

	private void PrepareDataDirectory()
	{
		if (Directory.Exists(DataDirectoryPath))
			Directory.Delete(DataDirectoryPath, true);
		Directory.CreateDirectory(DataDirectoryPath);
		_logger.Information("Created data directory: {DataDirectoryPath}", DataDirectoryPath);
	}

	private async Task ExportDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken)
	{
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(DataDirectoryPath), dataSet.ItemClasses);
		await _dataSetConfigurationExporter.Export(DataSetPath, dataSetParameters, cancellationToken);
	}
	
	private async Task ExportWeights(Weights weights, CancellationToken cancellationToken)
	{
		var data = await _weightsDataAccess.LoadWeightsData(weights, WeightsFormat.PT, cancellationToken);
		await File.WriteAllBytesAsync(WeightsToResumeTrainingOnPath, data.Content, cancellationToken);
	}
	
	private async Task<Weights> SaveWeights(Domain.Model.DataSet dataSet, TrainingProgress lastProgress, ModelSize size, CancellationToken cancellationToken)
	{
		var runDirectory = Directory.GetDirectories(RunsDirectoryPath).Single();
		var ptModelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(ptModelPath, dataSet.Resolution, _logger);
		var onnxData = await File.ReadAllBytesAsync(onnxModelPath, cancellationToken);
		var ptData = await File.ReadAllBytesAsync(ptModelPath, cancellationToken);
		var weights = await _weightsDataAccess.CreateWeights(dataSet.WeightsLibrary, onnxData, ptData, size, lastProgress.CurrentEpoch,
			lastProgress.BoundingLoss, lastProgress.ClassificationLoss, lastProgress.DeformationLoss, dataSet.Assets, cancellationToken);
		_logger.Information("Saved weights: {Weights}", weights);
		return weights;
	}
}