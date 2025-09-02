namespace SightKeeper.Application;

public interface ObservableRepository<out T>
{
	IObservable<T> Added { get; }
	IObservable<T> Removed { get; }
}