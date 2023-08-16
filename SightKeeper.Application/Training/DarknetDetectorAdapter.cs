using System.Reactive.Linq;
using Serilog;
using SightKeeper.Application.Training.Data;
using SightKeeper.Application.Training.Images;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Training;

public sealed class DarknetDetectorAdapter : DarknetAdapter<DetectorDataSet>
{
    public IObservable<TrainingProgress> Progress { get; }
    public int? MaxBatches { get; private set; }

    public DarknetDetectorAdapter(ImagesExporter<DetectorDataSet> imagesExporter, DarknetProcess process, DarknetOutputParser<DetectorDataSet> parser, ILogger? logger = null)
    {
        _logger = logger.WithGlobal();
        _imagesExporter = imagesExporter;
        _process = process;
        Progress = process.OutputReceived
            .Select(output => parser.TryParse(output, out var progress) ? progress : null)
            .WhereNotNull();
    }
    
    public async Task<byte[]?> RunAsync(DetectorDataSet dataSet, ModelConfig config, byte[]? baseWeights = null, CancellationToken cancellationToken = default)
    {
        ClearData();
        EnsureDirectoriesCreated();
        var images = await PrepareImagesAsync(dataSet, cancellationToken);
        await PrepareImagesListFileAsync(images);
        await PrepareClassesListAsync(dataSet.ItemClasses, cancellationToken);
        await PrepareDataFileAsync((byte)dataSet.ItemClasses.Count, cancellationToken);
        await PrepareConfigAsync(dataSet, config, out var maxBatches, cancellationToken);
        MaxBatches = maxBatches;
        DarknetArguments arguments = new()
        {
            ModelType = ModelType.Detector,
            DataPath = DarknetPaths.DataFilePath.Replace(DarknetPaths.DarknetDirectory, string.Empty),
            ConfigPath = DarknetPaths.ConfigFilePath.Replace(DarknetPaths.DarknetDirectory, string.Empty),
            DoNotShow = false
        };
        if (baseWeights != null)
        {
            await ExportBaseWeightsAsync(baseWeights, cancellationToken);
            arguments.BaseWeightsPath = DarknetPaths.BaseWeightsPath;
        }

        await _process.RunAsync(arguments, cancellationToken);
        var weightsFileContent = await GetLastWeightsFileContentAsync();
        ClearData();
        return weightsFileContent;
    }

    private static async Task<byte[]?> GetLastWeightsFileContentAsync(CancellationToken cancellationToken = default)
    {
        var weightsFilePath = GetLastWeightsFilePath();
        if (weightsFilePath == null) return null;
        var weightsFileContent = await File.ReadAllBytesAsync(weightsFilePath, cancellationToken);
        return weightsFileContent;
    }

    private async Task<IReadOnlyCollection<string>> PrepareImagesAsync(DetectorDataSet dataSet, CancellationToken cancellationToken = default)
    {
        var images = await _imagesExporter.ExportAsync(DarknetPaths.ImagesDirectoryPath, dataSet, cancellationToken);
        _logger.Information("Exported {Count} images", images.Count);
        return images;
    }

    private readonly ImagesExporter<DetectorDataSet> _imagesExporter;
    private readonly DarknetProcess _process;
    private readonly ILogger _logger;

    private static string? GetLastWeightsFilePath() =>
        new DirectoryInfo(DarknetPaths.OutputDirectoryPath).GetFiles().MaxBy(f => f.CreationTime)?.FullName;
    
    private static void EnsureDirectoriesCreated()
    {
        Directory.CreateDirectory(DarknetPaths.ImagesDirectoryPath);	
        Directory.CreateDirectory(DarknetPaths.OutputDirectoryPath);	
    }
	
    private static Task PrepareClassesListAsync(IEnumerable<ItemClass> itemClasses, CancellationToken cancellationToken)
    {
        var fileContent = string.Join('\n', itemClasses.Select(itemClass => itemClass.Name));
        return File.WriteAllTextAsync(DarknetPaths.ClassesListFilePath, fileContent, cancellationToken);
    }

    private static Task PrepareDataFileAsync(byte classesCount, CancellationToken cancellationToken)
    {
        DarknetData data = new()
        {
            ClassesCount = classesCount,
            ImagesListPath = DarknetPaths.ImagesListFilePath.Replace(DarknetPaths.DarknetDirectory, string.Empty),
            ClassesListPath = DarknetPaths.ClassesListFilePath.Replace(DarknetPaths.DarknetDirectory, string.Empty),
            OutputPath = DarknetPaths.OutputDirectoryPath.Replace(DarknetPaths.DarknetDirectory, string.Empty)
        };
        return File.WriteAllTextAsync(DarknetPaths.DataFilePath, data.ToString(), cancellationToken);
    }

    private static Task PrepareConfigAsync(DataSet dataSet, ModelConfig config, out int maxBatches, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        /*DetectorConfigParameters parameters = new(64, 16, (ushort)dataSet.Resolution.Width, (ushort)dataSet.Resolution.Height, (ushort) dataSet.ItemClasses.Count);
        maxBatches = parameters.MaxBatches;
        var fileContent = parameters.Deploy(config);
        return File.WriteAllTextAsync(DarknetPaths.ConfigFilePath, fileContent, cancellationToken);*/
    }

    private static Task PrepareImagesListFileAsync(IEnumerable<string> paths) => File.WriteAllLinesAsync(
        DarknetPaths.ImagesListFilePath,
        paths.Select(path => path.Replace(DarknetPaths.DarknetDirectory, string.Empty)));

    private static void ClearData()
    {
        if (Directory.Exists(DarknetPaths.DarknetDataDirectoryPath))
            Directory.Delete(DarknetPaths.DarknetDataDirectoryPath, true);
    }

    private static async Task ExportBaseWeightsAsync(byte[] weights, CancellationToken cancellationToken) =>
        await File.WriteAllBytesAsync(DarknetPaths.DarknetBaseWeightsPath, weights, cancellationToken);
}