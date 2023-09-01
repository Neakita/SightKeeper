using System.Reactive;
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
		CLITrainerArguments arguments = new(DataSetPath, size, epochs, dataSet.Resolution);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), _logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.Materialize()
			.Where(notification => notification.Kind != NotificationKind.OnCompleted)
			.Dematerialize()
			.Subscribe(trainingProgressObserver);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			_logger.Information("No training progress found");
			return null;
		}
		_logger.Debug("Last progress: {Progress}", lastProgress);
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
	private readonly ILogger _logger;

	private void PrepareDataDirectory()
	{
		Directory.Delete(DataDirectoryPath, true);
		Directory.CreateDirectory(DataDirectoryPath);
		_logger.Information("Created data directory: {DataDirectoryPath}", DataDirectoryPath);
	}

	private async Task ExportDataSet(Domain.Model.DataSet dataSet, CancellationToken cancellationToken)
	{
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(DataDirectoryPath), dataSet.ItemClasses);
		await _dataSetConfigurationExporter.Export(DataSetPath, dataSetParameters, cancellationToken);
		_logger.Information("Exported dataset parameters: {Parameters}", dataSetParameters);
	}
	
	private async Task<Weights> SaveWeights(Domain.Model.DataSet dataSet, TrainingProgress lastProgress, ModelSize size)
	{
		var runDirectory = Directory.GetDirectories(RunsDirectoryPath).Single();
		var modelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(modelPath, dataSet.Resolution, _logger);
		var bytes = await File.ReadAllBytesAsync(onnxModelPath, CancellationToken.None);
		var weights = await _weightsDataAccess.CreateWeights(dataSet.WeightsLibrary, bytes, size, lastProgress.CurrentEpoch,
			lastProgress.BoundingLoss, lastProgress.ClassificationLoss, lastProgress.DeformationLoss, dataSet.Assets, CancellationToken.None);
		_logger.Information("Saved weights: {Weights}", weights);
		return weights;
	}
}