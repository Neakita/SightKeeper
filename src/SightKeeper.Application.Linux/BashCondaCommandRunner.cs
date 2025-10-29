namespace SightKeeper.Application.Linux;

internal class BashCondaCommandRunner(CommandRunner inner, CondaLocator condaLocator) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		var condaActivationBatchFilePath = condaLocator.CondaActivationScriptPath;
		command = $"source {condaActivationBatchFilePath} && {command}";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}