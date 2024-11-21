using System.ComponentModel;
using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class DetectorContextViewModel : DataSetContextViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override ScreenshotsViewModel<DetectorAssetViewModel, DetectorAsset> Screenshots { get; }
	public override DetectorToolBarViewModel ToolBar { get; }
	public override DetectorDrawerViewModel Drawer { get; }

	public DetectorContextViewModel(
		DetectorDataSet dataSet,
		ObservableDataAccess<Screenshot> observableScreenshotsDataAccess,
		ScreenshotImageLoader imageLoader,
		DetectorAnnotator detectorAnnotator)
	{
		Screenshots = new ScreenshotsViewModel<DetectorAssetViewModel, DetectorAsset>(
			dataSet.ScreenshotsLibrary,
			observableScreenshotsDataAccess,
			imageLoader);
		ToolBar = new DetectorToolBarViewModel(dataSet.TagsLibrary.Tags);
		Drawer = new DetectorDrawerViewModel(detectorAnnotator);
		Screenshots.PropertyChanged += OnScreenshotsPropertyChanged;
		ToolBar.PropertyChanged += OnToolBarPropertyChanged;
	}

	private void OnScreenshotsPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(Screenshots.SelectedScreenshot))
			Drawer.Screenshot = (ScreenshotViewModel<DetectorAssetViewModel, DetectorAsset>?)Screenshots.SelectedScreenshot;
	}

	private void OnToolBarPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(ToolBar.Tag))
			Drawer.SetTag(ToolBar.Tag);
	}
}