using System.Diagnostics;
using Serilog;
using Serilog.Core;

namespace SightKeeper.Application.Extensions;

public static class CLIExtensions
{
    private static readonly ProcessStartInfo StartInfo = new()
    {
        FileName = "cmd.exe",
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
    };
    
    public static IObservable<string?> RunCLICommand(string arguments, ILogger? logger = null, CancellationToken cancellationToken = default)
    {
        logger ??= Logger.None;
        Process process = new();
        process.EnableRaisingEvents = true;
        process.StartInfo = StartInfo;
        process.Start();
        cancellationToken.Register(() =>
        {
            process.Kill();
            logger.Debug("Process terminated");
        });
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        _ = process.PassArguments(arguments);
        var outputDataStream = process.ObserveDataReceived();
        outputDataStream.WhereNotNull().Subscribe(data => logger.Verbose("[CLI] {Data}", data));
        return outputDataStream;
    }
    
    public static async Task PassArguments(this Process process, string arguments, ILogger? logger = null)
    {
        await process.StandardInput.WriteLineAsync(arguments);
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();
        logger?.Debug("Arguments passed: {Arguments}", arguments);
    }
}