using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed partial class AnnotatorViewModel : ViewModel, IAnnotatorViewModel
{
	public IReadOnlyCollection<DataSetViewModel> DataSetViewModels { get; }

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }
	public AnnotatorToolsViewModel ToolsViewModel { get; }
	public DrawerViewModel DrawerViewModel { get; }

	public bool CanChangeSelectedDataSet => !Screenshoter.IsEnabled;

	public AnnotatorViewModel(
		ScreenshoterViewModel screenshoterViewModel,
		AnnotatorScreenshotsViewModel screenshots,
		DataSetsListViewModel dataSetsListViewModel,
		AnnotatorToolsViewModel toolsViewModel,
		DrawerViewModel drawerViewModel)
	{
		Screenshoter = screenshoterViewModel;
		Screenshots = screenshots;
		ToolsViewModel = toolsViewModel;
		DrawerViewModel = drawerViewModel;
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedDataSet)));
		DataSetViewModels = dataSetsListViewModel.DataSets;
	}

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		ToolsViewModel.DataSetViewModel = value;
		DrawerViewModel.DataSetViewModel = value;
		Screenshoter.DataSet = value?.DataSet;
		Screenshots.DataSet = value?.DataSet;
	}
}