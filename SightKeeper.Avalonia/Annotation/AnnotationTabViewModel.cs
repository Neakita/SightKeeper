using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
		PendingScreenshotsCountReporter? pendingScreenshotsReporter,
		WriteableBitmapPool bitmapPool)
	{
		_bitmapPool = bitmapPool;
		DataSets = dataSets.DataSets;
		Screenshots = screenshots;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
	}

	private readonly WriteableBitmapPool _bitmapPool;
	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		var screenshotsLibrary = value?.DataSet.ScreenshotsLibrary;
		Screenshots.Library = screenshotsLibrary;
		ScreenshottingSettings.Library = screenshotsLibrary;
	}

	[RelayCommand(CanExecute = nameof(CanReturnBitmapToPool))]
	private void ReturnBitmapToPool(WriteableBitmap bitmap)
	{
		_bitmapPool.Return(bitmap);
	}

	private static bool CanReturnBitmapToPool(WriteableBitmap? bitmap)
	{
		return bitmap != null;
	}
}