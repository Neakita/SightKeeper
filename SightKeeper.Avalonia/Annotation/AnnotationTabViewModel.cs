using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation.DataSetContexts;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ushort> PendingScreenshotsCount { get; }
	public DataSetContextViewModel? Context
	{
		get;
		private set => SetProperty(ref field, value);
	}

	public AnnotationTabViewModel(
		DataSetsListViewModel dataSets,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingScreenshotsCountReporter? pendingScreenshotsReporter,
		WriteableBitmapPool bitmapPool,
		ObservableDataAccess<Screenshot> screenshotsDataAccess,
		ScreenshotImageLoader imageLoader,
		ClassifierAnnotator classifierAnnotator,
		DetectorAnnotator detectorAnnotator)
	{
		_bitmapPool = bitmapPool;
		_screenshotsDataAccess = screenshotsDataAccess;
		_imageLoader = imageLoader;
		_classifierAnnotator = classifierAnnotator;
		_detectorAnnotator = detectorAnnotator;
		DataSets = dataSets.DataSets;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly ObservableDataAccess<Screenshot> _screenshotsDataAccess;
	private readonly ScreenshotImageLoader _imageLoader;
	private readonly ClassifierAnnotator _classifierAnnotator;
	private readonly DetectorAnnotator _detectorAnnotator;
	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		var screenshotsLibrary = value?.DataSet.ScreenshotsLibrary;
		if (Context is IDisposable disposable)
			disposable.Dispose();
		Context = DataSetContextViewModel.Create(value?.DataSet, _screenshotsDataAccess, _imageLoader, _classifierAnnotator, _detectorAnnotator);
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