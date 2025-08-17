namespace SightKeeper.Application.Training;

public interface SessionCommandRunner : IDisposable, IAsyncDisposable
{
	IObservable<string> Output { get; }
	void ExecuteCommand(string command);
}