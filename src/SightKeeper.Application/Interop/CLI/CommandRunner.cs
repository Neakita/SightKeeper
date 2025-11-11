namespace SightKeeper.Application.Interop.CLI;

public interface CommandRunner
{
	Task ExecuteCommandAsync(string command, CancellationToken cancellationToken);
}