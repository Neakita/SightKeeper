namespace SightKeeper.Application.Windows;

public sealed class WindowsArgumentCarryCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		command = $"/C {command}";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}