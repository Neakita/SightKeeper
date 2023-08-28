using System.Diagnostics;

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
    
    public static IObservable<string?> RunCLICommand(string arguments)
    {
        Process process = new();
        process.EnableRaisingEvents = true;
        process.StartInfo = StartInfo;
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        _ = process.PassArguments(arguments);
        return process.ObserveDataReceived();
    }
    
    public static async Task PassArguments(this Process process, string arguments)
    {
        await process.StandardInput.WriteLineAsync(arguments);
        await process.StandardInput.FlushAsync();
        process.StandardInput.Close();
    }
}