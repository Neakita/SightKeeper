using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Diagnostics;
using Serilog;

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
    
    public static async Task<IAsyncDisposable> TemporarilyReplaceRunsDirectory(string directory)
    {
        var backupValue = await GetRunsDirectory();
        if (backupValue == directory)
            return new AsyncDisposable();
        await SetRunsDirectory(directory);
        return new AsyncDisposable(() => SetRunsDirectory(backupValue));
    }

    private static async Task<string> GetRunsDirectory()
    {
        var runsDirectoryParameter = await CLIExtensions.RunCLICommand("yolo settings")
            .WhereNotNull()
            .FirstAsync(output => output.StartsWith("runs_dir"));
        var runsDirectory = runsDirectoryParameter.Replace("runs_dir: ", string.Empty);
        Log.Debug("Retrieved runs directory: {Directory}", runsDirectory);
        return runsDirectory;
    }

    private static async Task SetRunsDirectory(string directory)
    {
        var runsDirectoryParameter = await CLIExtensions.RunCLICommand($"yolo settings runs_dir={directory}")
            .WhereNotNull()
            .FirstAsync(output => output.StartsWith("runs_dir"));
        var runsDirectory = runsDirectoryParameter.Replace("runs_dir: ", string.Empty);
        if (runsDirectory != directory)
            ThrowHelper.ThrowInvalidOperationException($"Failed to set runs directory, current value: {runsDirectory}");
        Log.Debug("The path to the runs directory \"{Directory}\" is set", directory);
    }

    public static async Task<string> ExportToONNX(string modelPath, ushort imagesSize)
    {
        var outputStream = CLIExtensions.RunCLICommand($"yolo export model=\"{modelPath}\" format=onnx imgsz={imagesSize} opset=15");
        await outputStream.WhereNotNull().Where(content =>
        {
            Log.Debug("Content: {Content}", content);
            return content.Contains("export success");
        }).FirstAsync();
        var onnxModelPath = modelPath.Replace(".pt", ".onnx");
        Guard.IsTrue(File.Exists(onnxModelPath));
        return onnxModelPath;
    }

    private static readonly Regex ONNXModelPathRegex = new(@"(?<=ONNX: export success  .*s, saved as ').*(?=\' \(.* MB\))");
}