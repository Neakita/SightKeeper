using Vibrance.Changes;

namespace SightKeeper.Data;

public interface ReadOnlyObservableCollection<out T> : IReadOnlyCollection<T>, IObservable<Change<T>>, IDisposable;