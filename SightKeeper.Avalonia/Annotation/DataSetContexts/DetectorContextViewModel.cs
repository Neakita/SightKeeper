using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Drawing.Detector;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class DetectorContextViewModel : DataSetContextViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override ScreenshotsViewModel<DetectorAssetViewModel, DetectorAsset> Screenshots { get; }
	public override ToolBarViewModel<DetectorAssetViewModel, DetectorAsset> ToolBar { get; }
	public override DrawerViewModel<DetectorAssetViewModel> Drawer { get; }

	public DetectorContextViewModel(
		DetectorDataSet dataSet,
		ObservableDataAccess<Screenshot> observableScreenshotsDataAccess,
		ScreenshotImageLoader imageLoader)
	{
		Screenshots = new ScreenshotsViewModel<DetectorAssetViewModel, DetectorAsset>(
			dataSet.ScreenshotsLibrary,
			observableScreenshotsDataAccess,
			imageLoader);
		ToolBar = new DetectorToolBarViewModel();
		Drawer = new DetectorDrawerViewModel();
		Initialize();
	}
}