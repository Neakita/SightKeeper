using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;
using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

internal sealed class AnnotatorViewModel : ViewModel, IDisposable
{
	public IReadOnlyCollection<DataSetViewModel> DataSetViewModels { get; }

	public AnnotatorScreenshotsViewModel Screenshots { get; }

	public ScreenshoterViewModel Screenshoter { get; }
	public AnnotatorToolsViewModel ToolsViewModel { get; }
	public Drawer.DrawerViewModel DrawerViewModel { get; }
	public SelectedDataSetViewModel SelectedDataSet { get; }
	public AutoAnnotationViewModel AutoAnnotationViewModel { get; }
	public ViewSettingsViewModel ViewSettingsViewModel { get; }

	public bool CanChangeSelectedDataSet => !Screenshoter.IsEnabled;

	public AnnotatorViewModel(
		ScreenshoterViewModel screenshoterViewModel,
		AnnotatorScreenshotsViewModel screenshots,
		DataSetsListViewModel dataSetsListViewModel,
		AnnotatorToolsViewModel toolsViewModel,
		Drawer.DrawerViewModel drawerViewModel,
		SelectedDataSetViewModel selectedDataSet,
		AutoAnnotationViewModel autoAnnotationViewModel,
		ViewSettingsViewModel viewSettingsViewModel)
	{
		Screenshoter = screenshoterViewModel;
		Screenshots = screenshots;
		ToolsViewModel = toolsViewModel;
		DrawerViewModel = drawerViewModel;
		SelectedDataSet = selectedDataSet;
		AutoAnnotationViewModel = autoAnnotationViewModel;
		ViewSettingsViewModel = viewSettingsViewModel;
		screenshoterViewModel.IsEnabledChanged.Subscribe(_ =>
			OnPropertyChanged(nameof(CanChangeSelectedDataSet)));
		DataSetViewModels = dataSetsListViewModel.DataSets;
		SelectedDataSet.ObservableValue.Subscribe(newValue =>
		{
			Screenshoter.DataSet = newValue?.DataSet;
			Screenshots.DataSet = newValue?.DataSet;
		}).DisposeWith(_disposable);
	}

	public void Dispose()
	{
		_disposable.Dispose();
	}

	private readonly CompositeDisposable _disposable = new();
}