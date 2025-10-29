namespace SightKeeper.Application.Linux;

internal sealed class BashArgumentCarryCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		command = $"-c \"{command}\"";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}