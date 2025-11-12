using SightKeeper.Application.Interop.CLI;

namespace SightKeeper.Application.Windows;

internal sealed class WindowsArgumentCarryCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken)
	{
		command = $"/C {command}";
		return inner.ExecuteCommandAsync(command, outputObserver, errorObserver, cancellationToken);
	}
}