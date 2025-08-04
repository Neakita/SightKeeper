using CliWrap;
using Serilog;
using SightKeeper.Application.Training;

namespace SightKeeper.Application.Linux;

public sealed class LinuxCommandRunner : CommandRunner
{
	private static readonly ILogger Logger = Log.ForContext<LinuxCommandRunner>();

	public Task ExecuteCommandAsync(string command)
	{
		Logger.Debug("Executing command: {command}", command);
		return Cli.Wrap("/bin/bash")
			.WithArguments(["-c", command])
			.WithStandardOutputPipe(PipeTarget.ToDelegate(Logger.Verbose))
			.WithStandardErrorPipe(PipeTarget.ToDelegate(Logger.Warning))
			.WithWorkingDirectory(Directory.GetCurrentDirectory())
			.ExecuteAsync();
	}
}