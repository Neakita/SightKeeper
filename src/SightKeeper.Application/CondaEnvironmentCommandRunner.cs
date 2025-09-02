namespace SightKeeper.Application;

public sealed class CondaEnvironmentCommandRunner : CommandRunner
{
	public CondaEnvironmentCommandRunner(CommandRunner inner, string environmentDirectoryPath)
	{
		_activateEnvironmentCommand = $"conda activate {environmentDirectoryPath}";
		_inner = inner;
	}

	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		command = $"{_activateEnvironmentCommand} && {command}";
		return _inner.ExecuteCommandAsync(command, cancellationToken);
	}

	private readonly string _activateEnvironmentCommand;
	private readonly CommandRunner _inner;
}