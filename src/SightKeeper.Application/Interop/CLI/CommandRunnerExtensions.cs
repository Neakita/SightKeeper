namespace SightKeeper.Application.Interop.CLI;

internal static class CommandRunnerExtensions
{
	public static Task ExecuteCommandAsync(this CommandRunner commandRunner, string command, CancellationToken cancellationToken)
	{
		return commandRunner.ExecuteCommandAsync(command, null, null, cancellationToken);
	}
}