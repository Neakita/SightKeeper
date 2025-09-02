namespace SightKeeper.Application.Windows;

public sealed class WindowsCondaCommandRunner(CommandRunner inner) : CommandRunner
{
	public Task ExecuteCommandAsync(string command, CancellationToken cancellationToken)
	{
		var condaActivationBatchFilePath = CondaLocator.CondaActivationBatchFilePath;
		command = $"CALL {condaActivationBatchFilePath} && {command}";
		return inner.ExecuteCommandAsync(command, cancellationToken);
	}
}