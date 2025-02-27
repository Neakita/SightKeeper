using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.ScreenCapturing.Saving;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class SideBarViewModel : ViewModel, AnnotationSideBarComponent
{
	private readonly ToolingViewModelFactory _toolingViewModelFactory;
	public IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	[ObservableProperty] public partial DataSetViewModel? SelectedDataSet { get; set; }
	[ObservableProperty] public partial ScreenshotsLibraryViewModel? SelectedScreenshotsLibrary { get; set; }
	public IObservable<ushort> PendingScreenshotsCount { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ScreenshotsLibraryViewModel?> SelectedScreenshotsLibraryChanged => _selectedScreenshotsLibraryChanged.AsObservable();
	public IObservable<DataSetViewModel?> SelectedDataSetChanged => _selectedDataSetChanged.AsObservable();
	[ObservableProperty] public partial object? AdditionalTooling { get; private set; }
	public IObservable<object?> AdditionalToolingChanged => _additionalToolingChanged.AsObservable();

	public SideBarViewModel(
		DataSetViewModelsObservableRepository dataSets,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingImagesCountReporter? pendingScreenshotsReporter,
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibraries,
		ToolingViewModelFactory toolingViewModelFactory)
	{
		_toolingViewModelFactory = toolingViewModelFactory;
		DataSets = dataSets.Items;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingImagesCount ?? Observable.Empty<ushort>();
		ScreenshotsLibraries = screenshotsLibraries.Items;
	}

	partial void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		_selectedScreenshotsLibraryChanged.OnNext(value);
		ScreenshottingSettings.Library = value?.Value;
	}

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		_selectedDataSetChanged.OnNext(value);
		AdditionalTooling = _toolingViewModelFactory.CreateToolingViewModel(value?.Value);
	}

	partial void OnAdditionalToolingChanged(object? value)
	{
		_additionalToolingChanged.OnNext(value);
	}

	private readonly Subject<ScreenshotsLibraryViewModel?> _selectedScreenshotsLibraryChanged = new();
	private readonly Subject<DataSetViewModel?> _selectedDataSetChanged = new();
	private readonly Subject<object?> _additionalToolingChanged = new();
}