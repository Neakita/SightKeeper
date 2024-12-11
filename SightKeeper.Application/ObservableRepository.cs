using System.Collections.ObjectModel;
using DynamicData;

namespace SightKeeper.Application;

public abstract class ObservableRepository<T> where T : notnull
{
	public abstract ReadOnlyObservableCollection<T> Items { get; }
	public abstract IObservableList<T> Source { get; }
}