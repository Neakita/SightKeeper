using CliWrap;
using Serilog;

namespace SightKeeper.Application.Interop.CLI;

public class ArgumentCommandRunner(string shellPath) : CommandRunner
{
	static ArgumentCommandRunner()
	{
		var logger = Log.ForContext<ArgumentCommandRunner>();
		OutputLogPipe = PipeTarget.ToDelegate(output => logger.Verbose("{output}", output));
		ErrorLogPipe = PipeTarget.ToDelegate(error => logger.Error("{error}", error));
	}

	public Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken)
	{
		Log.Debug("Executing {command}", command);
		var outputPipe = OutputLogPipe;
		var errorPipe = ErrorLogPipe;
		if (outputObserver != null)
			outputPipe = PipeTarget.Merge(PipeTarget.ToDelegate(outputObserver.OnNext), outputPipe);
		if (errorObserver != null)
			errorPipe = PipeTarget.Merge(PipeTarget.ToDelegate(errorObserver.OnNext), errorPipe);
		outputPipe = PipeTarget.Merge(outputPipe, OutputLogPipe);
		errorPipe = PipeTarget.Merge(errorPipe, ErrorLogPipe);
		return Cli.Wrap(shellPath)
			.WithWorkingDirectory(WorkingDirectory)
			.WithArguments(command)
			.WithStandardOutputPipe(outputPipe)
			.WithStandardErrorPipe(errorPipe)
			.ExecuteAsync(cancellationToken);
	}

	private static readonly string WorkingDirectory = Directory.GetCurrentDirectory();
	private static readonly PipeTarget OutputLogPipe;
	private static readonly PipeTarget ErrorLogPipe;
}