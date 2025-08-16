using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Domain.DataSets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

internal class DataSetsViewModel : ViewModel, DataSetsDataContext, IDisposable
{
	public IReadOnlyCollection<DataSetCardDataContext> DataSets => _dataSets.ToReadOnlyNotifyingList();

	public ICommand CreateDataSetCommand { get; }

	public DataSetsViewModel(
		ObservableListRepository<DataSet> dataSetsObservableListRepository,
		CreateDataSetCommand createDataSetCommand,
		DataSetCardViewModelFactory dataSetCardViewModelFactory)
	{
		var dataSets = dataSetsObservableListRepository.Items
			.Transform(dataSetCardViewModelFactory.CreateDataSetCardViewModel)
			.ToObservableList();
		_dataSets = dataSets;
		CreateDataSetCommand = createDataSetCommand;
	}

	private readonly ReadOnlyObservableList<DataSetCardDataContext> _dataSets;

	public void Dispose()
	{
		_dataSets.Dispose();
	}
}