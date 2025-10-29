namespace SightKeeper.Application.Linux;

internal class BashCondaCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		var condaActivationBatchFilePath = CondaLocator.CondaActivationBatchFilePath;
		command = $"source {condaActivationBatchFilePath} && {command}";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}