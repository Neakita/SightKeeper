using SightKeeper.Application.Interop.CLI;

namespace SightKeeper.Application.Interop.Conda;

public interface CondaEnvironmentManager
{
	Task<CommandRunner> ActivateAsync(
		string environmentDirectoryPath,
		string pythonVersion,
		CancellationToken cancellationToken);
}