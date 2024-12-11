using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Application;

public sealed class DataAccessObservableRepository<T> : ObservableRepository<T>, IDisposable where T : notnull
{
	public override ReadOnlyObservableCollection<T> Items { get; }
    public override IObservableList<T> Source => _source.AsObservableList();

    public DataAccessObservableRepository(ObservableDataAccess<T> observableDataAccess, ReadDataAccess<T> readDataAccess)
    {
	    observableDataAccess.Added.Subscribe(_source.Add).DisposeWith(_disposable);
	    observableDataAccess.Removed.Subscribe(item => Guard.IsTrue(_source.Remove(item))).DisposeWith(_disposable);
	    _source.AddRange(readDataAccess.Items);
	    _source.Connect().Bind(out var items).Subscribe().DisposeWith(_disposable);
	    Items = items;
    }

    public void Dispose()
    {
        _source.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<T> _source = new();
    private readonly CompositeDisposable _disposable = new();
}