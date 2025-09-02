namespace SightKeeper.Application;

public interface CommandRunner
{
	Task ExecuteCommandAsync(string command, CancellationToken cancellationToken);
}