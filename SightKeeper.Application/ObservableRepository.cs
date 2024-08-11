using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Application;

public sealed class ObservableRepository<T> : IDisposable where T : notnull
{
    public IObservableList<T> DataSetsSource => _source;

    public ObservableRepository(ObservableDataAccess<T> observableDataAccess, ReadDataAccess<T> readDataAccess)
    {
	    observableDataAccess.Added.Subscribe(_source.Add).DisposeWith(_disposable);
	    observableDataAccess.Removed.Subscribe(item => Guard.IsTrue(_source.Remove(item))).DisposeWith(_disposable);
	    _source.AddRange(readDataAccess.Items);
    }

    public void Dispose()
    {
        _source.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<T> _source = new();
    private readonly CompositeDisposable _disposable = new();
}