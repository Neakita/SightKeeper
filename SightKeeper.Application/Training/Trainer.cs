using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class Trainer
{
	private static readonly string DataDirectoryPath = Path.Combine("Data", "Training");
	private static readonly string DataSetPath = Path.Combine(DataDirectoryPath, "data.yaml");
	private static readonly string RunsDirectoryPath = Path.Combine(DataDirectoryPath, "Runs");
	private static readonly string WeightsToResumeTrainingOnPath = Path.Combine(DataDirectoryPath, "weights.pt");
	private static readonly ILogger Logger = Log.ForContext<Trainer>();

	public Trainer(
		AssetsExporter assetsExporter,
		DataSetConfigurationExporter dataSetConfigurationExporter,
		WeightsDataAccess weightsDataAccess,
		ObjectsLookupper objectsLookupper)
	{
		_assetsExporter = assetsExporter;
		_dataSetConfigurationExporter = dataSetConfigurationExporter;
		_weightsDataAccess = weightsDataAccess;
		_objectsLookupper = objectsLookupper;
	}

	public bool AMP { get; set; } = true;

	public async Task<Weights?> TrainFromScratchAsync(
		DataSet dataSet,
		ModelSize modelSize,
		uint epochs,
		ushort patience,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		_assetsExporter.Export(DataDirectoryPath, dataSet.Assets, dataSet.ItemClasses);
		await ExportDataSet(dataSet, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), Logger);
		CLITrainerArguments arguments = new(DataSetPath, modelSize, epochs, patience, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), Logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			Logger.Information("No training progress found");
			return null;
		}
		Logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress.WeightsMetrics, modelSize, CancellationToken.None); // TODO ability to abort saving
	}

	public async Task<Weights?> ResumeTrainingAsync(
		Weights weights,
		uint epochs,
		ushort patience,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		var dataSet = _objectsLookupper.GetDataSet(_objectsLookupper.GetLibrary(weights));
		_assetsExporter.Export(DataDirectoryPath, dataSet.Assets, dataSet.ItemClasses);
		await ExportDataSet(dataSet, cancellationToken);
		await ExportWeights(weights, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), Logger);
		CLITrainerArguments arguments = new(DataSetPath, Path.GetFullPath(WeightsToResumeTrainingOnPath), epochs, patience, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), Logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			Logger.Information("No training progress found");
			return null;
		}
		Logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress.WeightsMetrics, weights.Size, CancellationToken.None); // TODO ability to abort saving
	}

	private readonly AssetsExporter _assetsExporter;
	private readonly DataSetConfigurationExporter _dataSetConfigurationExporter;
	private readonly WeightsDataAccess _weightsDataAccess;
	private readonly ObjectsLookupper _objectsLookupper;

	private void PrepareDataDirectory()
	{
		if (Directory.Exists(DataDirectoryPath))
			Directory.Delete(DataDirectoryPath, true);
		Directory.CreateDirectory(DataDirectoryPath);
		Logger.Information("Created data directory: {DataDirectoryPath}", DataDirectoryPath);
	}

	private async Task ExportDataSet(DataSet dataSet, CancellationToken cancellationToken)
	{
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(DataDirectoryPath), dataSet.ItemClasses);
		await _dataSetConfigurationExporter.Export(DataSetPath, dataSetParameters, cancellationToken);
	}
	
	private async Task ExportWeights(Weights weights, CancellationToken cancellationToken)
	{
		var data = _weightsDataAccess.LoadWeightsPTData(weights);
		await File.WriteAllBytesAsync(WeightsToResumeTrainingOnPath, data.Content, cancellationToken);
	}
	
	private async Task<Weights> SaveWeights(DataSet dataSet, WeightsMetrics lastWeightsMetrics, ModelSize modelSize, CancellationToken cancellationToken)
	{
		var runDirectory = Directory.GetDirectories(RunsDirectoryPath).Single();
		var ptModelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(ptModelPath, dataSet.Resolution, Logger);
		var onnxData = await File.ReadAllBytesAsync(onnxModelPath, cancellationToken);
		var ptData = await File.ReadAllBytesAsync(ptModelPath, cancellationToken);
		var weights = _weightsDataAccess.CreateWeights(dataSet.Weights, onnxData, ptData, modelSize, lastWeightsMetrics, dataSet.ItemClasses);
		Logger.Information("Saved weights: {Weights}", weights);
		return weights;
	}
}