using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using Point = Avalonia.Point;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
	DetectorModel? Model { get; set; }
	DetectorScreenshot? SelectedScreenshot { get; set; }
	ItemClass? SelectedItemClass { get; set; }
	Task DeleteItemAsync(int itemIndex);
	void MarkAsAssets(IReadOnlyCollection<int> screenshotsIndexes);
	void DeleteScreenshots(IReadOnlyCollection<int> screenshotsIndexes);
	bool BeginDrawing(Point position);
	void UpdateDrawing(Point position);
	void EndDrawing(Point position);
}