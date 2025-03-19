using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using Vibrance;

namespace SightKeeper.Application;

public sealed class DataAccessObservableRepository<T> : ObservableRepository<T>, IDisposable where T : notnull
{
	public ReadOnlyObservableList<T> Items => _items;

    public DataAccessObservableRepository(ObservableDataAccess<T> observableDataAccess, ReadDataAccess<T> readDataAccess)
    {
	    observableDataAccess.Added.Subscribe(_items.Add).DisposeWith(_disposable);
	    observableDataAccess.Removed.Subscribe(item => Guard.IsTrue(_items.Remove(item))).DisposeWith(_disposable);
	    _items.AddRange(readDataAccess.Items);
    }

    public void Dispose()
    {
        _items.Dispose();
        _disposable.Dispose();
    }

    private readonly ObservableList<T> _items = new();
    private readonly CompositeDisposable _disposable = new();
}