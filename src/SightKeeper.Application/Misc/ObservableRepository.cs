namespace SightKeeper.Application.Misc;

public interface ObservableRepository<out T>
{
	IObservable<T> Added { get; }
	IObservable<T> Removed { get; }
}