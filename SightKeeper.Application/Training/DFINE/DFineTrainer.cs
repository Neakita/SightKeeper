using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application.Training.DFINE;

public sealed class DFineTrainer(CommandRunner commandRunner, CondaEnvironmentManager environmentManager, DataSetTrainerExporter<DetectorDataSet> exporter) : Trainer<DetectorDataSet>
{
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

	public async Task<Weights> TrainAsync(
		DetectorDataSet dataSet,
		AssetsDistributionRequest assetsDistributionRequest,
		CancellationToken cancellationToken)
	{
		var environmentCommandRunner = await environmentManager.ActivateAsync(CondaEnvironmentPath, PythonVersion, cancellationToken);
		await InstallDFineAsync(environmentCommandRunner, cancellationToken);
		var tagsCount = dataSet.TagsLibrary.Tags.Count;
		var model = DFineModel.Nano;
		await Configurator.ConfigureAsync(16, new Vector2<ushort>(320, 320), (byte)tagsCount, model, OutputDirectoryPath, cancellationToken);
		exporter.DirectoryPath = DataSetPath;
		await exporter.ExportAsync(dataSet, assetsDistributionRequest, cancellationToken);
		var configFilePath = Path.Combine(ModelConfigsDirectoryPath, model.ConfigName);
		await environmentCommandRunner.ExecuteCommandAsync($"python {TrainPythonScriptPath} -c {configFilePath} --seed=0", cancellationToken);
		return null!;
	}

	private async Task InstallDFineAsync(CommandRunner environmentCommandRunner, CancellationToken cancellationToken)
	{
		if (!Directory.Exists(RepositoryPath))
			await commandRunner.ExecuteCommandAsync($"git clone https://github.com/Peterande/D-FINE.git {RepositoryPath}", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync("pip install torch torchvision --index-url https://download.pytorch.org/whl/cu129", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync($"pip install -r {RequirementsPath}", cancellationToken);
		await environmentCommandRunner.ExecuteCommandAsync("pip install matplotlib", cancellationToken);
	}
}