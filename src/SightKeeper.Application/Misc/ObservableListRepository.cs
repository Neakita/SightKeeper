using Vibrance;

namespace SightKeeper.Application.Misc;

public interface ObservableListRepository<out T>
{
	ReadOnlyObservableList<T> Items { get; }
}