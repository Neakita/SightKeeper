using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModelsObservableRepository : ObservableRepository<DataSetViewModel>, IDisposable
{
	public override ReadOnlyObservableCollection<DataSetViewModel> Items { get; }
	public override IObservableList<DataSetViewModel> Source => _source.AsObservableList();

    public DataSetViewModelsObservableRepository(ObservableRepository<DataSet> repository)
    {
	    _disposable = repository.Source.Connect()
		    .Transform(dataSet => new DataSetViewModel(dataSet))
		    .DisposeMany()
		    .Bind(out var items)
		    .PopulateInto(_source);
        Items = items;
    }

    public void Dispose()
    {
	    _disposable.Dispose();
	    _source.Dispose();
    }

    private readonly IDisposable _disposable;
    private readonly SourceList<DataSetViewModel> _source = new();
}