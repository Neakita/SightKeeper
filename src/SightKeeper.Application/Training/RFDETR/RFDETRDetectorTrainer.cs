using System.Reactive.Subjects;
using System.Text;
using Serilog;
using SightKeeper.Application.Interop.CLI;
using SightKeeper.Application.Interop.Conda;

namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETRDetectorTrainer(
	CondaEnvironmentManager environmentManager,
	ILogger logger,
	IObserver<object> progressObserver)
	: Trainer, OutputProvider, OptionsHolder, TrainingPathsProvider
{
	public RFDETRTrainingOptions TrainingOptions { get; set; } = new();
	public IObservable<string> Output => _output;
	public TrainingOptions Options => TrainingOptions;
	public string OutputDirectoryPath => Path.Combine(WorkingDirectory, "artifacts");

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
		using var output = new Subject<string>();
		await environmentCommandRunner.ExecuteCommandAsync(TrainCommand, _output, null, cancellationToken);
		progressObserver.OnNext("Training completed");
	}

	private static readonly string WorkingDirectory = Path.Combine("environments", "RF-DETR");
	private static readonly string CondaEnvironmentPath = Path.Combine(WorkingDirectory, "conda-environment");
	internal static readonly string DataSetPath = Path.Combine(WorkingDirectory, "dataset");
	private static readonly string TrainPythonScriptPath = Path.Combine("Training", "RFDETR", "train.py");

	private readonly Subject<string> _output = new();
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
}