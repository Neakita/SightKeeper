namespace SightKeeper.Application.Interop.CLI;

public sealed class CondaEnvironmentCommandRunner(CommandRunner inner, string environmentDirectoryPath) : CommandRunner
{
	public Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken)
	{
		command = $"{_activateEnvironmentCommand} && {command}";
		return inner.ExecuteCommandAsync(command, outputObserver, errorObserver, cancellationToken);
	}

	private readonly string _activateEnvironmentCommand = $"conda activate {environmentDirectoryPath}";
}