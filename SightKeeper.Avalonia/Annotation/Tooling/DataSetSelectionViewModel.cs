using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class DataSetSelectionViewModel : ViewModel, DataSetSelectionDataContext, DataSetSelection, IDisposable
{
	public IReadOnlyCollection<DataSetViewModel> DataSets { get; }
	[ObservableProperty] public partial DataSetViewModel? SelectedDataSet { get; set; }
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged.AsObservable();

	public DataSetSelectionViewModel(DataSetViewModelsObservableListRepository dataSetsListRepository)
	{
		DataSets = dataSetsListRepository.Items;
	}

	IReadOnlyCollection<DataSetDataContext> DataSetSelectionDataContext.DataSets => DataSets;

	DataSetDataContext? DataSetSelectionDataContext.SelectedDataSet
	{
		get => SelectedDataSet;
		set => SelectedDataSet = (DataSetViewModel?)value;
	}

	DataSet? DataSetSelection.SelectedDataSet => SelectedDataSet?.Value;

	IObservable<DataSet?> DataSetSelection.SelectedDataSetChanged =>
		SelectedDataSetChanged.Select(dataSetViewModel => dataSetViewModel?.Value);

	public void Dispose()
	{
		_selectedDataSetChanged.Dispose();
	}

	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetChanged.OnNext(value);
	}
}