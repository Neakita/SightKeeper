using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser2D;
using SightKeeper.Domain.Model.DataSets.Poser3D;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed partial class AnnotationTabViewModel : ViewModel
{
	public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	public ScreenshotsViewModel? Screenshots
	{
		get;
		private set => SetProperty(ref field, value);
	}
	public ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	public IObservable<ushort> PendingScreenshotsCount { get; }

	public AnnotationTabViewModel(
		DataSetsListViewModel dataSets,
		ScreenshottingSettingsViewModel screenshottingSettings,
		PendingScreenshotsCountReporter? pendingScreenshotsReporter,
		WriteableBitmapPool bitmapPool,
		ObservableDataAccess<Screenshot> screenshotsDataAccess,
		ScreenshotImageLoader imageLoader)
	{
		_bitmapPool = bitmapPool;
		_screenshotsDataAccess = screenshotsDataAccess;
		_imageLoader = imageLoader;
		DataSets = dataSets.DataSets;
		ScreenshottingSettings = screenshottingSettings;
		PendingScreenshotsCount = pendingScreenshotsReporter?.PendingScreenshotsCount ?? Observable.Empty<ushort>();
	}

	private readonly WriteableBitmapPool _bitmapPool;
	private readonly ObservableDataAccess<Screenshot> _screenshotsDataAccess;
	private readonly ScreenshotImageLoader _imageLoader;
	[ObservableProperty] private DataSetViewModel? _selectedDataSet;

	partial void OnSelectedDataSetChanged(DataSetViewModel? value)
	{
		var screenshotsLibrary = value?.DataSet.ScreenshotsLibrary;
		Screenshots = screenshotsLibrary switch
		{
			null => null,
			ScreenshotsLibrary<ClassifierAsset> classifier => new ScreenshotsViewModel<ClassifierAssetViewModel, ClassifierAsset>(classifier, _screenshotsDataAccess, _imageLoader),
			ScreenshotsLibrary<DetectorAsset> detector => new ScreenshotsViewModel<DetectorAssetViewModel, DetectorAsset>(detector, _screenshotsDataAccess, _imageLoader),
			ScreenshotsLibrary<Poser2DAsset> poser2D => new ScreenshotsViewModel<Poser2DAssetViewModel, Poser2DAsset>(poser2D, _screenshotsDataAccess, _imageLoader),
			ScreenshotsLibrary<Poser3DAsset> poser3D => new ScreenshotsViewModel<Poser3DAssetViewModel, Poser3DAsset>(poser3D, _screenshotsDataAccess, _imageLoader),
			_ => throw new ArgumentOutOfRangeException(nameof(screenshotsLibrary), screenshotsLibrary, null)
		};
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