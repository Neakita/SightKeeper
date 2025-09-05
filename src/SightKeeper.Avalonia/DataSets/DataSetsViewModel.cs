using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

internal class DataSetsViewModel : ViewModel, DataSetsDataContext, IDisposable
{
	public IReadOnlyCollection<DataSetCardDataContext> DataSets => _dataSets.ToReadOnlyNotifyingList();

	public ICommand CreateDataSetCommand { get; }
	public ICommand ImportDataSetCommand { get; }

	public DataSetsViewModel(
		ObservableListRepository<DataSet<Asset>> dataSetsObservableListRepository,
		CreateDataSetCommand createDataSetCommand,
		ImportDataSetCommand importDataSetCommand,
		DataSetCardViewModelFactory dataSetCardViewModelFactory)
	{
		var dataSets = dataSetsObservableListRepository.Items
			.Transform(dataSetCardViewModelFactory.CreateDataSetCardViewModel)
			.ToObservableList();
		_dataSets = dataSets;
		CreateDataSetCommand = createDataSetCommand;
		ImportDataSetCommand = importDataSetCommand;
	}

	private readonly ReadOnlyObservableList<DataSetCardDataContext> _dataSets;

	public void Dispose()
	{
		_dataSets.Dispose();
	}
}