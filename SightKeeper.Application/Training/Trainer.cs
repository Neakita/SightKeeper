using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Weights;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application.Training;

public sealed class Trainer(
	ImagesExporter imagesExporter,
	DataSetConfigurationExporter dataSetConfigurationExporter,
	WeightsDataAccess weightsDataAccess)
{
	private static readonly string DataDirectoryPath = Path.Combine("Data", "Training");
	private static readonly string DataSetPath = Path.Combine(DataDirectoryPath, "data.yaml");
	private static readonly string RunsDirectoryPath = Path.Combine(DataDirectoryPath, "Runs");
	private static readonly string WeightsToResumeTrainingOnPath = Path.Combine(DataDirectoryPath, "weights.pt");
	private static readonly ILogger Logger = Log.ForContext<Trainer>();

	public bool AMP { get; set; } = true;

	public async Task<Weights?> TrainFromScratchAsync(
		Domain.Model.DataSet.DataSet dataSet,
		ModelSize size,
		uint epochs,
		ushort patience,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		await imagesExporter.Export(DataDirectoryPath, dataSet, cancellationToken);
		await ExportDataSet(dataSet, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), Logger);
		CLITrainerArguments arguments = new(DataSetPath, size, epochs, patience, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), Logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			Logger.Information("No training progress found");
			return null;
		}
		Logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress, size, CancellationToken.None); // TODO ability to abort saving
	}

	public async Task<Weights?> ResumeTrainingAsync(
		Weights weights,
		uint epochs,
		ushort patience,
		IObserver<TrainingProgress> trainingProgressObserver,
		CancellationToken cancellationToken = default)
	{
		PrepareDataDirectory();
		var dataSet = weights.Library.DataSet;
		await imagesExporter.Export(DataDirectoryPath, dataSet, cancellationToken);
		await ExportDataSet(dataSet, cancellationToken);
		await ExportWeights(weights, cancellationToken);
		await using var runsDirectoryReplacement = await YoloCLIExtensions.TemporarilyReplaceRunsDirectory(Path.GetFullPath(RunsDirectoryPath), Logger);
		CLITrainerArguments arguments = new(DataSetPath, Path.GetFullPath(WeightsToResumeTrainingOnPath), epochs, patience, dataSet.Resolution, false, AMP);
		var outputStream = CLIExtensions.RunCLICommand(arguments.ToString(), Logger, cancellationToken);
		TrainerParser.Parse(outputStream.WhereNotNull(), out var trainingProgress);
		using var trainingProgressObserverDisposable = trainingProgress
			.IgnoreCompletion()
			.Subscribe(trainingProgressObserver);
		await runsDirectoryReplacement.DisposeAsync();
		var lastProgress = await trainingProgress.LastOrDefaultAsync();
		if (lastProgress == null)
		{
			Logger.Information("No training progress found");
			return null;
		}
		Logger.Debug("Last progress: {Progress}", lastProgress);
		return await SaveWeights(dataSet, lastProgress, weights.Size, CancellationToken.None); // TODO ability to abort saving
	}

	private void PrepareDataDirectory()
	{
		if (Directory.Exists(DataDirectoryPath))
			Directory.Delete(DataDirectoryPath, true);
		Directory.CreateDirectory(DataDirectoryPath);
		Logger.Information("Created data directory: {DataDirectoryPath}", DataDirectoryPath);
	}

	private async Task ExportDataSet(Domain.Model.DataSet.DataSet dataSet, CancellationToken cancellationToken)
	{
		DataSetConfigurationParameters dataSetParameters = new(Path.GetFullPath(DataDirectoryPath), dataSet.ItemClasses);
		await dataSetConfigurationExporter.Export(DataSetPath, dataSetParameters, cancellationToken);
	}
	
	private async Task ExportWeights(Weights weights, CancellationToken cancellationToken)
	{
		var data = await weightsDataAccess.LoadWeightsData(weights, WeightsFormat.PT, cancellationToken);
		await File.WriteAllBytesAsync(WeightsToResumeTrainingOnPath, data.Content, cancellationToken);
	}
	
	private async Task<Weights> SaveWeights(Domain.Model.DataSet.DataSet dataSet, TrainingProgress lastProgress, ModelSize size, CancellationToken cancellationToken)
	{
		var runDirectory = Directory.GetDirectories(RunsDirectoryPath).Single();
		var ptModelPath = Path.Combine(runDirectory, "train", "weights", "last.pt");
		var onnxModelPath = await YoloCLIExtensions.ExportToONNX(ptModelPath, dataSet.Resolution, Logger);
		var onnxData = await File.ReadAllBytesAsync(onnxModelPath, cancellationToken);
		var ptData = await File.ReadAllBytesAsync(ptModelPath, cancellationToken);
		var weights = await weightsDataAccess.CreateWeights(dataSet.WeightsLibrary, onnxData, ptData, size, lastProgress.CurrentEpoch,
			lastProgress.BoundingLoss, lastProgress.ClassificationLoss, lastProgress.DeformationLoss, dataSet.ItemClasses, cancellationToken);
		Logger.Information("Saved weights: {Weights}", weights);
		return weights;
	}
}