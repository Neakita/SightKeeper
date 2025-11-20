using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using CommunityToolkit.Diagnostics;
using Serilog;
using SightKeeper.Application.Interop.CLI;
using SightKeeper.Application.Interop.Conda;
using SightKeeper.Application.Misc;

namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETRDetectorTrainer(
	CondaEnvironmentManager environmentManager,
	ILogger logger,
	IObserver<object> progressObserver) : Trainer
{
	public RFDETRTrainingOptions TrainingOptions { get; set; } = new();

	public async Task TrainAsync(CancellationToken cancellationToken)
	{
		var environmentCommandRunner = await ActivateEnvironmentAsync(cancellationToken);
		await InstallRFDETRAsync(environmentCommandRunner, cancellationToken);
		await TrainAsync(environmentCommandRunner, cancellationToken);
	}

	private Task<CommandRunner> ActivateEnvironmentAsync(CancellationToken cancellationToken)
	{
		progressObserver.OnNext("Preparing environment");
		return environmentManager.ActivateAsync(CondaEnvironmentPath, null, cancellationToken);
	}

	private async Task TrainAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		progressObserver.OnNext("Starting training");
		logger.Information("Starting training");
		var outputParser = new RFDETROutputParser(logger.ForContext<RFDETROutputParser>());
		using var output = new Subject<string>();
		var epochResults = outputParser.Parse(output).Publish();
		using var epochResultsSubscription = epochResults.Connect();
		using var fileSystemWatcherTrainingArtifactsProvider = new FileSystemWatcherTrainingArtifactsProvider(
			OutputDirectoryPath,
			"*.pth",
			epochResults,
			logger.ForContext<FileSystemWatcherTrainingArtifactsProvider>());
		_timeEstimator = new RemainingTimeEstimator(TrainingOptions.Epochs);
		using var progressSubscription = epochResults
			.Select(ToProgress)
			.Subscribe(progressObserver);
		await environmentCommandRunner.ExecuteCommandAsync(TrainCommand, output, null, cancellationToken);
		progressObserver.OnNext("Training completed");
	}

	private static readonly string WorkingDirectory = Path.Combine("environments", "RF-DETR");
	private static readonly string CondaEnvironmentPath = Path.Combine(WorkingDirectory, "conda-environment");
	internal static readonly string DataSetPath = Path.Combine(WorkingDirectory, "dataset");
	private static readonly string TrainPythonScriptPath = Path.Combine("Training", "RFDETR", "train.py");
	private static readonly string OutputDirectoryPath = Path.Combine(WorkingDirectory, "artifacts");

	private RemainingTimeEstimator? _timeEstimator;
	private string TrainCommand
	{
		get
		{
			var builder = new StringBuilder();
			builder.AppendJoin(' ',
				"python", TrainPythonScriptPath,
				"--model", TrainingOptions.Model.Argument,
				"--dataset_dir", DataSetPath,
				"--epochs", TrainingOptions.Epochs,
				"--batch_size", TrainingOptions.BatchSize,
				"--grad_accum_steps", TrainingOptions.GradientAccumulationSteps,
				"--output_dir", OutputDirectoryPath,
				"--resolution", TrainingOptions.Resolution);
			return builder.ToString();
		}
	}

	private async Task InstallRFDETRAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		logger.Information("Installing RF-DETR");
		await environmentCommandRunner.ExecuteCommandAsync("pip install rfdetr", cancellationToken);
	}

	private Progress ToProgress(EpochResult epochResult)
	{
		Guard.IsNotNull(_timeEstimator);
		return new Progress
		{
			Label = "Training",
			Total = TrainingOptions.Epochs,
			Current = epochResult.EpochNumber + 1,
			EstimatedTimeOfArrival = DateTime.Now + _timeEstimator.Estimate(epochResult.EpochNumber + 1)
		};
	}
}