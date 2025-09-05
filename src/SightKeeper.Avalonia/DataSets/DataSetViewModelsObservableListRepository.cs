using System;
using SightKeeper.Application;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

public sealed class DataSetViewModelsObservableListRepository : ObservableListRepository<DataSetViewModel>, IDisposable
{
	public ReadOnlyObservableList<DataSetViewModel> Items { get; }

    public DataSetViewModelsObservableListRepository(ObservableListRepository<DataSet<Asset>> listRepository)
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