using SightKeeper.Application.Interop.CLI;

namespace SightKeeper.Application.Linux;

internal sealed class BashArgumentCarryCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken)
	{
		command = $"-c \"{command}\"";
		return inner.ExecuteCommandAsync(command, outputObserver, errorObserver, cancellationToken);
	}
}