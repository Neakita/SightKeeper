using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public AnnotationScreenshotsViewModel Screenshots { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }

	public AnnotationTabViewModel(
		DataSetsListViewModel dataSets,
		AnnotationScreenshotsViewModel screenshots,
		ScreenshottingSettingsViewModel screenshottingSettings)
	{
		DataSets = dataSets.DataSets;
		Screenshots = screenshots;
		ScreenshottingSettings = screenshottingSettings;
	}

	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		var screenshotsLibrary = value?.DataSet.ScreenshotsLibrary;
		Screenshots.Library = screenshotsLibrary;
		ScreenshottingSettings.Library = screenshotsLibrary;
	}
}