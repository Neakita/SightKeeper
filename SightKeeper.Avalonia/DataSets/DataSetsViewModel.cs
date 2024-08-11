using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.DataSets;

internal partial class DataSetsViewModel : ViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }

	public DataSetsViewModel(DataSetsListViewModel dataSetsListViewModel)
	{
		DataSets = dataSetsListViewModel.DataSets;
	}

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;
}