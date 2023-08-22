using System;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorViewModel : ViewModel, IAnnotatingViewModel
{
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged;
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }

	public AnnotatorEnvironmentHolder EnvironmentHolder { get; }

	public bool CanChangeSelectedDataSet => !Screenshoter.IsEnabled;

	public AnnotatorViewModel(
		ScreenshoterViewModel screenshoterViewModel,
		AnnotatorScreenshotsViewModel screenshots,
		DataSetsListViewModel dataSetsListViewModel,
		AnnotatorEnvironmentHolder environmentHolder,
		AnnotatorSelectedDataSetHolder selectedDataSetHolder)
	{
		Screenshoter = screenshoterViewModel;
		_selectedDataSetHolder = selectedDataSetHolder;
		Screenshots = screenshots;
		EnvironmentHolder = environmentHolder;
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedDataSet)));
		DataSets = dataSetsListViewModel.DataSets;
	}

	private readonly AnnotatorSelectedDataSetHolder _selectedDataSetHolder;
	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetHolder.SelectedDataSetViewModel = value;
		_selectedDataSetChanged.OnNext(value);
	}
}