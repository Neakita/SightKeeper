using SightKeeper.Application.Training.Data;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training.DFINE;

public sealed class DFineTrainer(CommandRunner commandRunner, CondaEnvironmentManager environmentManager, TrainDataExporter<DetectorDataSet> exporter) : Trainer<DetectorDataSet>
{
	public byte BatchSize { get; set; } = 16;
	public Vector2<ushort> ImageSize { get; set; } = new(320, 320);
	public DFineModel Model { get; set; } = DFineModel.Nano;

	public async Task<Weights> TrainAsync(TrainData<DetectorDataSet> data, CancellationToken cancellationToken)
	{
		var environmentCommandRunner = await environmentManager.ActivateAsync(CondaEnvironmentPath, PythonVersion, cancellationToken);
		await InstallDFineAsync(environmentCommandRunner, cancellationToken);
		var tagsCount = data.Tags.Count();
		await Configurator.ConfigureAsync(BatchSize, ImageSize, (byte)tagsCount, Model, OutputDirectoryPath, cancellationToken);
		await exporter.ExportAsync(DataSetPath, data, cancellationToken);
		var configFilePath = Path.Combine(ModelConfigsDirectoryPath, Model.ConfigName);
		await environmentCommandRunner.ExecuteCommandAsync($"python {TrainPythonScriptPath} -c {configFilePath} --seed=0", cancellationToken);
		return null!;
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
	private static readonly DFineConfigurator Configurator = new(RepositoryPath, DataSetPath);

	private async Task InstallDFineAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		if (!Directory.Exists(RepositoryPath))
			await commandRunner.ExecuteCommandAsync($"git clone https://github.com/Peterande/D-FINE.git {RepositoryPath}", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync("pip install torch torchvision --index-url https://download.pytorch.org/whl/cu129", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync($"pip install -r {RequirementsPath}", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync("pip install matplotlib", cancellationToken);
	}
}