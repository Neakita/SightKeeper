using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.DataSets.Card;
using SightKeeper.Avalonia.DataSets.Commands;
using SightKeeper.Domain.DataSets;
using Vibrance.Changes;

namespace SightKeeper.Avalonia.DataSets;

internal class DataSetsViewModel : ViewModel, DataSetsDataContext, IDisposable
{
	public IReadOnlyCollection<DataSetCardDataContext> DataSets { get; }
	public ICommand CreateDataSetCommand { get; }

	public DataSetsViewModel(
		ObservableRepository<DataSet> dataSetsObservableRepository,
		CreateDataSetCommandFactory createDataSetCommandFactory,
		DataSetCardViewModelFactory dataSetCardViewModelFactory)
	{
		var dataSets = dataSetsObservableRepository.Items
			.Transform(dataSetCardViewModelFactory.CreateDataSetCardViewModel)
			.ToObservableList();
		DataSets = dataSets;
		_disposable = dataSets;
		CreateDataSetCommand = createDataSetCommandFactory.CreateCommand();
	}

	private readonly IDisposable _disposable;

	public void Dispose()
	{
		_disposable.Dispose();
	}
}