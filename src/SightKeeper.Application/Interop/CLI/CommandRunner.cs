namespace SightKeeper.Application.Interop.CLI;

public interface CommandRunner
{
	Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken);
}