using Serilog;

namespace SightKeeper.Application;

public sealed class StatelessCondaEnvironmentManager(CommandRunner condaCommandRunner, ILogger logger) : CondaEnvironmentManager
{
	public async Task<CommandRunner> ActivateAsync(string environmentDirectoryPath, string pythonVersion, CancellationToken cancellationToken)
	{
		logger.Information("Preparing environment {environmentDirectoryPath}", environmentDirectoryPath);
		await EnsureEnvironmentExistsAsync(environmentDirectoryPath, pythonVersion, cancellationToken);
		return new CondaEnvironmentCommandRunner(condaCommandRunner, environmentDirectoryPath);
	}

	private Task EnsureEnvironmentExistsAsync(string environmentDirectoryPath, string pythonVersion, CancellationToken cancellationToken)
	{
		logger.Debug("Ensuring {environmentDirectoryPath} environment exists", environmentDirectoryPath);
		if (EnvironmentExists(environmentDirectoryPath))
			return Task.CompletedTask;
		return CreateEnvironmentAsync(environmentDirectoryPath, pythonVersion, cancellationToken);
	}

	private static bool EnvironmentExists(string environmentDirectoryPath)
	{
		return Directory.Exists(environmentDirectoryPath) &&
		       Directory.EnumerateFileSystemEntries(environmentDirectoryPath).Any();
	}

	private Task CreateEnvironmentAsync(string directoryPath, string? pythonVersion, CancellationToken cancellationToken)
	{
		logger.Information("Creating environment {directoryPath} with python version {pythonVersion}", directoryPath, pythonVersion);
		var createCommand = $"conda create --prefix {directoryPath} --yes";
		if (pythonVersion != null)
			createCommand += $" python={pythonVersion}";
		return condaCommandRunner.ExecuteCommandAsync(createCommand, cancellationToken);
	}
}