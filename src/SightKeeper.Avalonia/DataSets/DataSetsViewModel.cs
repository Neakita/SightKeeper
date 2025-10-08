using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using Vibrance;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

internal class DataSetsViewModel(
	ObservableListRepository<DataSet<Tag, Asset>> dataSetsObservableListRepository,
	ICommand createDataSetCommand,
	ICommand importDataSetCommand,
	Func<DataSet<Tag, Asset>, DataSetCardViewModel> dataSetCardViewModelFactory)
	: ViewModel, DataSetsDataContext, IDisposable
{
	public IReadOnlyCollection<DataSetCardDataContext> DataSets => _dataSets.ToReadOnlyNotifyingList();
	public ICommand CreateDataSetCommand => createDataSetCommand;
	public ICommand ImportDataSetCommand => importDataSetCommand;

	public void Dispose()
	{
		_dataSets.Dispose();
	}

	private readonly ReadOnlyObservableList<DataSetCardDataContext> _dataSets =
		dataSetsObservableListRepository.Items
			.Transform(dataSetCardViewModelFactory)
			.ToObservableList();
}