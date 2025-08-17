using SightKeeper.Application.Training.Assets.Distribution;
using SightKeeper.Application.Training.COCO;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Weights;
using YamlDotNet.RepresentationModel;

namespace SightKeeper.Application.Training;

public sealed class DFineTrainer(COCODetectorDataSetExporter exporter) : Trainer<DetectorDataSet>
{
	private static readonly string WorkingDirectory = Path.GetFullPath(Path.Combine("environments", "D-FINE"));
	private static readonly string CondaEnvironmentPath = Path.Combine(WorkingDirectory, "conda-environment");
	private static readonly string RepositoryPath = Path.Combine(WorkingDirectory, "repository");
	private static readonly string RequirementsPath = Path.Combine(RepositoryPath, "requirements.txt");
	private static readonly string DataSetPath = Path.Combine(WorkingDirectory, "dataset");
	private static readonly string ConfigPath = Path.Combine(RepositoryPath, "configs", "dataset", "custom_detection.yml");
	private static readonly string AdjustedConfigPath = Path.Combine(RepositoryPath, "configs", "dataset", "adjusted_custom_detection.yml");
	private static readonly string TrainImagesDirectoryPath = Path.Combine(DataSetPath, "train");
	private static readonly string TrainDataSetFilePath = Path.Combine(DataSetPath, "train.json");
	private static readonly string ValidationImagesDirectoryPath = Path.Combine(DataSetPath, "validation");
	private static readonly string ValidationDataSetFilePath = Path.Combine(DataSetPath, "validation.json");

	public async Task<Weights> TrainAsync(
		DetectorDataSet dataSet,
		AssetsDistributionRequest assetsDistributionRequest,
		CancellationToken cancellationToken)
	{
		await using var commandRunner = new LinuxSessionCommandRunner(WorkingDirectory);
		await CreateEnvironmentAsync(commandRunner);
		InstallDFine(commandRunner);
		await AdjustConfig(0, cancellationToken);
		exporter.DirectoryPath = DataSetPath;
		await exporter.ExportAsync(dataSet, assetsDistributionRequest, cancellationToken);
		commandRunner.ExecuteCommand("export model=n");
		commandRunner.ExecuteCommand("CUDA_VISIBLE_DEVICES=0,1,2,3 torchrun --master_port=7777 --nproc_per_node=4 train.py -c configs/dfine/dfine_hgnetv2_${model}_coco.yml --seed=0");
		return null!;
	}

	private static Task CreateEnvironmentAsync(SessionCommandRunner commandRunner)
	{
		return CondaVirtualEnvironment.ActivateOrCreateAsync(commandRunner, CondaEnvironmentPath, "3.11.9");
	}

	private static void InstallDFine(SessionCommandRunner commandRunner)
	{
		commandRunner.ExecuteCommand("git clone https://github.com/Peterande/D-FINE.git repository");
		commandRunner.ExecuteCommand($"pip install -r {RequirementsPath}");
	}

	private static async Task AdjustConfig(int tagsCount, CancellationToken cancellationToken)
	{
		await using var configStream = File.OpenRead(ConfigPath);
		var yaml = new YamlStream();
		using var streamReader = new StreamReader(configStream);
		yaml.Load(streamReader);

		var root = (YamlMappingNode)yaml.Documents.Single().RootNode;

		var tagsCountNode = (YamlScalarNode)root.Children.Single(pair => pair.Key is YamlScalarNode { Value: "num_classes" }).Value;
		tagsCountNode.Value = tagsCount.ToString();

		var trainDataLoader = (YamlMappingNode)root.Children.Single(pair => pair.Key is YamlScalarNode { Value: "train_dataloader" }).Value;
		var trainDataSet = (YamlMappingNode)trainDataLoader.Single(pair => pair.Key is YamlScalarNode { Value: "dataset" }).Value;
		var trainImagesDirectory = (YamlScalarNode)trainDataSet.Single(pair => pair.Key is YamlScalarNode { Value: "img_folder" }).Value;
		trainImagesDirectory.Value = TrainImagesDirectoryPath;
		var trainDataSetPath = (YamlScalarNode)trainDataSet.Single(pair => pair.Key is YamlScalarNode {Value: "ann_file"}).Value;
		trainDataSetPath.Value = TrainDataSetFilePath;

		var validationDataLoader = (YamlMappingNode)root.Children.Single(pair => pair.Key is YamlScalarNode { Value: "val_dataloader" }).Value;
		var validationDataSet = (YamlMappingNode)validationDataLoader.Single(pair => pair.Key is YamlScalarNode { Value: "dataset" }).Value;
		var validationImagesDirectory = (YamlScalarNode)validationDataSet.Single(pair => pair.Key is YamlScalarNode { Value: "img_folder" }).Value;
		validationImagesDirectory.Value = ValidationImagesDirectoryPath;
		var validationDataSetPath = (YamlScalarNode)validationDataSet.Single(pair => pair.Key is YamlScalarNode {Value: "ann_file"}).Value;
		validationDataSetPath.Value = ValidationDataSetFilePath;

		var adjustedConfigStream = File.OpenWrite(AdjustedConfigPath);
		await using var streamWriter = new StreamWriter(adjustedConfigStream);
		yaml.Save(streamWriter, false);
	}
}