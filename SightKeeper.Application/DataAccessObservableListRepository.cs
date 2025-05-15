using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using SightKeeper.Application.Extensions;
using Vibrance;

namespace SightKeeper.Application;

public sealed class DataAccessObservableListRepository<T> : ObservableListRepository<T>, IDisposable where T : notnull
{
	public ReadOnlyObservableList<T> Items => _items;

    public DataAccessObservableListRepository(ObservableRepository<T> observableRepository, ReadRepository<T> readRepository)
    {
	    observableRepository.Added.Subscribe(_items.Add).DisposeWith(_disposable);
	    observableRepository.Removed.Subscribe(item => Guard.IsTrue(_items.Remove(item))).DisposeWith(_disposable);
	    _items.AddRange(readRepository.Items);
    }

    public void Dispose()
    {
        _items.Dispose();
        _disposable.Dispose();
    }

    private readonly ObservableList<T> _items = new();
    private readonly CompositeDisposable _disposable = new();
}