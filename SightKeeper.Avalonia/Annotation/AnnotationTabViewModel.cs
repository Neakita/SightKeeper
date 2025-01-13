using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation.Contexts;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public ScreenshotsViewModel Screenshots { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ushort> PendingScreenshotsCount { get; }

	public DataSetAnnotationContext? Context
	{
		get;
		private set => SetProperty(ref field, value);
	}
	[ObservableProperty] public partial DataSetViewModel? SelectedDataSet { get; set; }
	[ObservableProperty] public partial ScreenshotsLibraryViewModel? SelectedScreenshotsLibrary { get; set; }

	public AnnotationTabViewModel(
		DataSetViewModelsObservableRepository dataSets,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingScreenshotsCountReporter? pendingScreenshotsReporter,
		WriteableBitmapPool bitmapPool,
		ScreenshotsViewModel screenshots,
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibraries,
		DataSetAnnotationContextFactory contextFactory)
	{
		_bitmapPool = bitmapPool;
		_contextFactory = contextFactory;
		Screenshots = screenshots;
		DataSets = dataSets.Items;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
		ScreenshotsLibraries = screenshotsLibraries.Items;
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly DataSetAnnotationContextFactory _contextFactory;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		Context = _contextFactory.ReuseContextOrCreateNew(Context, value?.Value);
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

	partial void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibraryViewModel? value)
	{
		Screenshots.Library = value?.Value;
		ScreenshottingSettings.Library = value?.Value;
	}
}