using System;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModelsObservableRepository : ObservableRepository<DataSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<DataSetViewModel> Items { get; }

    public DataSetViewModelsObservableRepository(ObservableRepository<DataSet> repository)
    {
	    Items = repository.Items
		    .Transform(dataSet => new DataSetViewModel(dataSet))
		    .DisposeMany()
		    .ToObservableList();
    }

    public void Dispose()
    {
	    Items.Dispose();
    }
}