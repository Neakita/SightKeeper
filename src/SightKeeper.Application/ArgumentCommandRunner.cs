using CliWrap;
using Serilog;

namespace SightKeeper.Application;

public class ArgumentCommandRunner(string shellPath) : CommandRunner
{
	static ArgumentCommandRunner()
	{
		var logger = Log.ForContext<ArgumentCommandRunner>();
		OutputPipeTarget = PipeTarget.ToDelegate(output => logger.Verbose("{output}", output));
		ErrorPipeTarget = PipeTarget.ToDelegate(error => logger.Error("{error}", error));
	}

	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		Log.Debug("Executing {command}", command);
		return Cli.Wrap(shellPath)
			.WithWorkingDirectory(WorkingDirectory)
			.WithArguments(command)
			.WithStandardOutputPipe(OutputPipeTarget)
			.WithStandardErrorPipe(ErrorPipeTarget)
			.ExecuteAsync(cancellationToken);
	}

	private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();
	private static readonly PipeTarget OutputPipeTarget;
	private static readonly PipeTarget ErrorPipeTarget;
}