using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Autofac;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation.DataSetContexts;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Screenshots;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public ScreenshotsViewModel Screenshots { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ushort> PendingScreenshotsCount { get; }
	public DataSetContextViewModel? Context
	{
		get;
		private set => SetProperty(ref field, value);
	}

	public AnnotationTabViewModel(
		DataSetViewModelsObservableRepository dataSets,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingScreenshotsCountReporter? pendingScreenshotsReporter,
		WriteableBitmapPool bitmapPool,
		IComponentContext componentContext,
		ScreenshotsViewModel screenshots,
		ScreenshotsLibraryViewModelsObservableRepository screenshotsLibraries)
	{
		_bitmapPool = bitmapPool;
		_componentContext = componentContext;
		Screenshots = screenshots;
		DataSets = dataSets.Items;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
		ScreenshotsLibraries = screenshotsLibraries.Items;
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly IComponentContext _componentContext;
	[ObservableProperty] private ScreenshotsLibrary? _selectedScreenshotsLibrary;
	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		if (Context is IDisposable disposable)
			disposable.Dispose();
		Context = DataSetContextViewModel.Create(value?.Value, _componentContext);
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

	partial void OnSelectedScreenshotsLibraryChanged(ScreenshotsLibrary? value)
	{
		Screenshots.Library = value;
		ScreenshottingSettings.Library = value;
	}
}