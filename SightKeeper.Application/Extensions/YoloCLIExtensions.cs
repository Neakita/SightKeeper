using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using Serilog;
using Serilog.Core;

namespace SightKeeper.Application.Extensions;

public static class YoloCLIExtensions
{
    private sealed class AsyncDisposable : IAsyncDisposable
    {
        public AsyncDisposable(Func<Task>? action = null)
        {
            _action = action;
            if (action == null)
                _disposed = true;
        }
        
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;
            if (_action != null)
                await _action();
            _disposed = true;
        }

        private readonly Func<Task>? _action;
        private bool _disposed;
    }
    
    public static async Task<IAsyncDisposable> TemporarilyReplaceRunsDirectory(string directory, ILogger? logger = null)
    {
        logger ??= Logger.None;
        var backupValue = await GetRunsDirectory(logger);
        if (backupValue == directory)
        {
            logger.Debug("The runs directory is already set to {Directory}, no operations are performed", directory);
            return new AsyncDisposable();
        }
        await SetRunsDirectory(directory, logger);
        logger.Debug("Runs directory temporarily set to {Directory}", directory);
        return new AsyncDisposable(async () =>
        {
            await SetRunsDirectory(backupValue, logger);
            logger.Debug("Runs directory restored to {Directory}", backupValue);
        });
    }

    private static async Task<string> GetRunsDirectory(ILogger logger)
    {
        var runsDirectoryParameter = await CLIExtensions.RunCLICommand("yolo settings", logger)
            .WhereNotNull()
            .FirstAsync(output => output.StartsWith("runs_dir"));
        var runsDirectory = runsDirectoryParameter.Replace("runs_dir: ", string.Empty);
        Log.Debug("Retrieved runs directory: {Directory}", runsDirectory);
        return runsDirectory;
    }

    private static async Task SetRunsDirectory(string directory, ILogger logger)
    {
        directory = ReplaceBackSlashes(directory);
        var arguments = $"yolo settings runs_dir=\'{directory}\'";
        var runsDirectoryParameter = await CLIExtensions.RunCLICommand(arguments, logger)
            .WhereNotNull()
            .FirstAsync(output => output.StartsWith("runs_dir"));
        var runsDirectory = runsDirectoryParameter.Replace("runs_dir: ", string.Empty);
        if (runsDirectory != directory)
            ThrowHelper.ThrowInvalidOperationException($"Failed to set runs directory to \"{directory}\", current value: {runsDirectory}");
        Log.Debug("The path to the runs directory \"{Directory}\" is set", directory);
    }

    public static async Task<string> ExportToONNX(string modelPath, ushort imagesSize, ILogger logger)
    {
        var fullModelPath = Path.GetFullPath(modelPath);
        var fullModelPathWithReplacedSlashes = ReplaceBackSlashes(fullModelPath);
        var outputStream = CLIExtensions.RunCLICommand($"yolo export model=\'{fullModelPathWithReplacedSlashes}\' format=onnx imgsz={imagesSize} opset=15", logger);
        await outputStream.WhereNotNull().Where(content => content.Contains("export success")).FirstAsync();
        var onnxModelPath = modelPath.Replace(".pt", ".onnx");
        Guard.IsTrue(File.Exists(onnxModelPath));
        return onnxModelPath;
    }

    private static string ReplaceBackSlashes(string path) =>
        BackSlashRegex.Replace(path, @"/");

    private static readonly Regex BackSlashRegex = new(@"\\", RegexOptions.Compiled);
}