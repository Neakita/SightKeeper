namespace SightKeeper.Application.Windows;

internal sealed class WindowsCondaCommandRunner(CommandRunner inner, CondaLocator condaLocator) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		var condaActivationBatchFilePath = condaLocator.CondaActivationScriptPath;
		command = $"CALL {condaActivationBatchFilePath} && {command}";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}