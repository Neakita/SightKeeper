namespace SightKeeper.Application.Training;

internal interface OutputProvider
{
	IObservable<string> Output { get; }
}