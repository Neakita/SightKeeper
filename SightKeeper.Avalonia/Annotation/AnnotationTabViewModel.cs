using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public ScreenshotsViewModel Screenshots { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ushort> PendingScreenshotsCount { get; }

	public AnnotationTabViewModel(
		DataSetsListViewModel dataSets,
		ScreenshotsViewModel screenshots,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingScreenshotsCountReporter? pendingScreenshotsReporter)
	{
		DataSets = dataSets.DataSets;
		Screenshots = screenshots;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
	}

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		var screenshotsLibrary = value?.DataSet.ScreenshotsLibrary;
		Screenshots.Library = screenshotsLibrary;
		ScreenshottingSettings.Library = screenshotsLibrary;
	}
}