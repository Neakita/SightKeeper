using SightKeeper.Application.Interop.CLI;
using SightKeeper.Application.Interop.Conda;

namespace SightKeeper.Application.Linux;

internal sealed class BashCondaCommandRunner(CommandRunner inner, CondaLocator condaLocator) : CommandRunner
{
	public Task ExecuteCommandAsync(
		string command,
		IObserver<string>? outputObserver,
		IObserver<string>? errorObserver,
		CancellationToken cancellationToken)
	{
		var condaActivationBatchFilePath = condaLocator.CondaActivationScriptPath;
		command = $"source {condaActivationBatchFilePath} && {command}";
		return inner.ExecuteCommandAsync(command, outputObserver, errorObserver, cancellationToken);
	}
}