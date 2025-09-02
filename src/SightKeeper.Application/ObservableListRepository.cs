using Vibrance;

namespace SightKeeper.Application;

public interface ObservableListRepository<out T>
{
	ReadOnlyObservableList<T> Items { get; }
}