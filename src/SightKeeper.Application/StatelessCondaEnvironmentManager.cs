namespace SightKeeper.Application;

public sealed class StatelessCondaEnvironmentManager(CommandRunner condaCommandRunner) : CondaEnvironmentManager
{
	public async Task<CommandRunner> ActivateAsync(string environmentDirectoryPath, string pythonVersion, CancellationToken cancellationToken)
	{
		await EnsureEnvironmentExistsAsync(condaCommandRunner, environmentDirectoryPath, pythonVersion, cancellationToken);
		return new CondaEnvironmentCommandRunner(condaCommandRunner, environmentDirectoryPath);
	}

	private static Task EnsureEnvironmentExistsAsync(CommandRunner condaCommandRunner, string environmentDirectoryPath, string pythonVersion, CancellationToken cancellationToken)
	{
		if (EnvironmentExists(environmentDirectoryPath))
			return Task.CompletedTask;
		return CreateEnvironmentAsync(condaCommandRunner, environmentDirectoryPath, pythonVersion, cancellationToken);
	}

	private static bool EnvironmentExists(string environmentDirectoryPath)
	{
		return Directory.Exists(environmentDirectoryPath) &&
		       Directory.EnumerateFileSystemEntries(environmentDirectoryPath).Any();
	}

	private static Task CreateEnvironmentAsync(CommandRunner condaCommandRunner, string directoryPath, string? pythonVersion, CancellationToken cancellationToken)
	{
		var createCommand = $"conda create --prefix {directoryPath} --yes";
		if (pythonVersion != null)
			createCommand += $" python={pythonVersion}";
		return condaCommandRunner.ExecuteCommandAsync(createCommand, cancellationToken);
	}
}