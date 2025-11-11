using System.Text;
using Serilog;
using SightKeeper.Application.Interop.CLI;
using SightKeeper.Application.Interop.Conda;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.RFDETR;

internal sealed class RFDETRDetectorTrainer(
	CondaEnvironmentManager environmentManager,
	TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> exporter,
	ILogger logger) : Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>
{
	public byte BatchSize { get; set; } = 4;
	public ushort Resolution { get; set; } = 320;
	public RFDETRModel Model { get; set; } = RFDETRModel.Nano;
	public ushort Epochs { get; set; } = 100;
	public byte GradientAccumulationSteps { get; set; } = 4;

	public async Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data,
		CancellationToken cancellationToken)
	{
		var environmentCommandRunner = await ActivateEnvironmentAsync(cancellationToken);
		await InstallRFDETRAsync(environmentCommandRunner, cancellationToken);
		await ExportData(data, cancellationToken);
		await StartTrainingAsync(cancellationToken, environmentCommandRunner);
	}

	private Task<CommandRunner> ActivateEnvironmentAsync(CancellationToken cancellationToken)
	{
		return environmentManager.ActivateAsync(CondaEnvironmentPath, PythonVersion, cancellationToken);
	}

	private async Task ExportData(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data,
		CancellationToken cancellationToken)
	{
		logger.Information("Exporting data");
		await exporter.ExportAsync(DataSetPath, data, cancellationToken);
	}

	public async Task StartTrainingAsync(CancellationToken cancellationToken, CommandRunner environmentCommandRunner)
	{
		logger.Information("Starting training");
		await environmentCommandRunner.ExecuteCommandAsync(TrainCommand, cancellationToken);
	}

	private const string PythonVersion = "3.11.9";
	private static readonly string WorkingDirectory = Path.Combine("environments", "RF-DETR");
	private static readonly string CondaEnvironmentPath = Path.Combine(WorkingDirectory, "conda-environment");
	private static readonly string DataSetPath = Path.Combine(WorkingDirectory, "dataset");
	private static readonly string TrainPythonScriptPath = Path.Combine("Training", "RFDETR", "train.py");
	private static readonly string OutputDirectoryPath = Path.Combine(WorkingDirectory, "artifacts");

	private string TrainCommand
	{
		get
		{
			var builder = new StringBuilder();
			builder.AppendJoin(' ',
				"python", TrainPythonScriptPath,
				"--model", Model.Argument,
				"--dataset_dir", DataSetPath,
				"--epochs", Epochs,
				"--batch_size", BatchSize,
				"--grad_accum_steps", GradientAccumulationSteps,
				"--output_dir", OutputDirectoryPath,
				"--resolution", Resolution);
			return builder.ToString();
		}
	}

	private async Task InstallRFDETRAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		logger.Information("Installing RF-DETR");
		await environmentCommandRunner.ExecuteCommandAsync("pip install rfdetr", cancellationToken);
	}
}