using System;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModelsObservableListRepository : ObservableListRepository<DataSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<DataSetViewModel> Items { get; }

    public DataSetViewModelsObservableListRepository(ObservableListRepository<DataSet> listRepository)
    {
	    Items = listRepository.Items
		    .Transform(dataSet => new DataSetViewModel(dataSet))
		    .DisposeMany()
		    .ToObservableList();
    }

    public void Dispose()
    {
	    Items.Dispose();
    }
}