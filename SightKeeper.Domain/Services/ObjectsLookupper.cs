using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Services;

public interface ObjectsLookupper
{
	ScreenshotsLibrary GetLibrary(Screenshot screenshot);
	WeightsLibrary GetLibrary(Weights weights);
	DetectorAssetsLibrary GetLibrary(DetectorAsset screenshot);
	IReadOnlyCollection<DetectorItem> GetItems(Tag tag);
	DetectorDataSet GetDataSet(Tag tag);
	DetectorAsset GetAsset(DetectorItem item);
	DetectorAsset? GetOptionalAsset(Screenshot screenshot);
	DetectorDataSet GetDataSet(WeightsLibrary weightsLibrary);
	DetectorAsset GetAsset(Screenshot screenshot);
	DetectorDataSet GetDataSet(ScreenshotsLibrary screenshotsLibrary);
}