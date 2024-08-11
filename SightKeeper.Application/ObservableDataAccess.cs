namespace SightKeeper.Application;

public interface ObservableDataAccess<out T>
{
	IObservable<T> Added { get; }
	IObservable<T> Removed { get; }
}