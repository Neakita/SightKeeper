using Vibrance;

namespace SightKeeper.Application;

public interface ObservableListRepository<out T> where T : notnull
{
	ReadOnlyObservableList<T> Items { get; }
}