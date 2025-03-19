using Vibrance;

namespace SightKeeper.Application;

public interface ObservableRepository<out T> where T : notnull
{
	ReadOnlyObservableList<T> Items { get; }
}