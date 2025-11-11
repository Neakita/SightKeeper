using Serilog;
using SightKeeper.Application.Interop.CLI;
using SightKeeper.Application.Interop.Conda;
using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Application.Training.DFINE;

internal sealed class DFineTrainer(
	CommandRunner commandRunner,
	CondaEnvironmentManager environmentManager,
	TrainDataExporter<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> exporter,
	ILogger logger)
	: Trainer<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>>
{
	public byte BatchSize { get; set; } = 16;
	public Vector2<ushort> ImageSize { get; set; } = new(320, 320);
	public DFineModel Model { get; set; } = DFineModel.Nano;

	public async Task TrainAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data, CancellationToken cancellationToken)
	{
		var environmentCommandRunner = await environmentManager.ActivateAsync(CondaEnvironmentPath, PythonVersion, cancellationToken);
		await InstallDFineAsync(environmentCommandRunner, cancellationToken);
		await ConfigureAsync(data, cancellationToken);
		await ExportData(data, cancellationToken);
		await StartTrainingAsync(cancellationToken, environmentCommandRunner);
	}

	private async Task ConfigureAsync(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data, CancellationToken cancellationToken)
	{
		var tagsCount = data.Tags.Count();
		await _configurator.ConfigureAsync(BatchSize, ImageSize, (byte)tagsCount, Model, OutputDirectoryPath, cancellationToken);
	}

	private async Task StartTrainingAsync(CancellationToken cancellationToken, CommandRunner environmentCommandRunner)
	{
		logger.Information("Starting training");
		var configFilePath = Path.Combine(ModelConfigsDirectoryPath, Model.ConfigName);
		await environmentCommandRunner.ExecuteCommandAsync($"python {TrainPythonScriptPath} -c {configFilePath} --seed=0", cancellationToken);
	}

	private async Task ExportData(ReadOnlyDataSet<ReadOnlyTag, ReadOnlyItemsAsset<ReadOnlyDetectorItem>> data, CancellationToken cancellationToken)
	{
		logger.Information("Exporting data");
		await exporter.ExportAsync(DataSetPath, data, cancellationToken);
	}

	private const string PythonVersion = "3.11.9";
	private static readonly string WorkingDirectory = Path.Combine("environments", "D-FINE");
	private static readonly string CondaEnvironmentPath = Path.Combine(WorkingDirectory, "conda-environment");
	private static readonly string RepositoryPath = Path.Combine(WorkingDirectory, "repository");
	private static readonly string RequirementsPath = Path.Combine(RepositoryPath, "requirements.txt");
	private static readonly string DataSetPath = Path.Combine(WorkingDirectory, "dataset");
	private static readonly string TrainPythonScriptPath = Path.Combine(RepositoryPath, "train.py");
	private static readonly string ModelConfigsDirectoryPath = Path.Combine(RepositoryPath, "configs", "dfine", "custom");
	private static readonly string OutputDirectoryPath = Path.Combine(WorkingDirectory, "artifacts");
	private readonly DFineConfigurator _configurator = new(RepositoryPath, DataSetPath, logger.ForContext<DFineConfigurator>());

	private async Task InstallDFineAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		logger.Information("Installing D-FINE");
		if (!Directory.Exists(RepositoryPath))
		{
			logger.Information("Downloading D-FINE repository");
			await commandRunner.ExecuteCommandAsync($"git clone https://github.com/Peterande/D-FINE.git {RepositoryPath}", cancellationToken);
		}

		logger.Information("Installing torch");
		await environmentCommandRunner.ExecuteCommandAsync("pip install torch torchvision --index-url https://download.pytorch.org/whl/cu129", cancellationToken);
		
		logger.Information("Installing requirements");
		await environmentCommandRunner.ExecuteCommandAsync($"pip install -r {RequirementsPath}", cancellationToken);
		
		logger.Information("Installing matplotlib");
		await environmentCommandRunner.ExecuteCommandAsync("pip install matplotlib", cancellationToken);
	}
}