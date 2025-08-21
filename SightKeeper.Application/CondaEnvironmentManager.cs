namespace SightKeeper.Application;

public interface CondaEnvironmentManager
{
	Task<CommandRunner> ActivateAsync(
		string environmentDirectoryPath,
		string pythonVersion,
		CancellationToken cancellationToken);
}