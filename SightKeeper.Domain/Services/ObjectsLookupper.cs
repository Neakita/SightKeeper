using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Services;

public interface ObjectsLookupper
{
	ScreenshotsLibrary GetLibrary(Screenshot screenshot);
	WeightsLibrary GetLibrary(Weights weights);
	AssetsLibrary GetLibrary(Asset screenshot);
	IReadOnlyCollection<DetectorItem> GetItems(ItemClass itemClass);
	DataSet GetDataSet(ItemClass itemClass);
	Asset GetAsset(DetectorItem item);
	Asset? GetOptionalAsset(Screenshot screenshot);
	DataSet GetDataSet(WeightsLibrary weightsLibrary);
	Asset GetAsset(Screenshot screenshot);
	DataSet GetDataSet(ScreenshotsLibrary screenshotsLibrary);
}