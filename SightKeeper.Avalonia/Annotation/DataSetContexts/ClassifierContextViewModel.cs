using SightKeeper.Application;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawers;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal sealed class ClassifierContextViewModel : DataSetContextViewModel<ClassifierAssetViewModel, ClassifierAsset>
{
	public override ScreenshotsViewModel<ClassifierAssetViewModel, ClassifierAsset> Screenshots { get; }
	public override ClassifierToolBarViewModel ToolBar { get; }
	public override DrawerViewModel<ClassifierAssetViewModel>? Drawer => null;

	public ClassifierContextViewModel(
		ClassifierDataSet dataSet,
		ObservableDataAccess<Screenshot> observableScreenshotsDataAccess,
		ScreenshotImageLoader imageLoader,
		ClassifierAnnotator classifierAnnotator)
	{
		Screenshots = new ScreenshotsViewModel<ClassifierAssetViewModel, ClassifierAsset>(
			dataSet.ScreenshotsLibrary,
			observableScreenshotsDataAccess,
			imageLoader);
		ToolBar = new ClassifierToolBarViewModel(dataSet.TagsLibrary.Tags, classifierAnnotator);
		Initialize();
	}
}