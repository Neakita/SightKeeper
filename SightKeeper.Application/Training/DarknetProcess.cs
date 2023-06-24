namespace SightKeeper.Application.Training;

public interface DarknetProcess
{
    IObservable<string> OutputReceived { get; }
    Task RunAsync(DarknetArguments arguments, CancellationToken cancellationToken = default);
}