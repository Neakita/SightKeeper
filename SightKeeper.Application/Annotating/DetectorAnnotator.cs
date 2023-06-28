using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
	DetectorModel? Model { get; set; }
	DetectorAsset? SelectedScreenshot { get; set; }
	ItemClass? SelectedItemClass { get; set; }
	Task DeleteItemAsync(int itemIndex);
	void MarkAsAssets(IReadOnlyCollection<int> screenshotsIndexes);
	void DeleteScreenshots(IReadOnlyCollection<int> screenshotsIndexes);
	bool BeginDrawing(Point position);
	void UpdateDrawing(Point position);
	void EndDrawing(Point position);
}