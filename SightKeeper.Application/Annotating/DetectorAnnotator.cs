using Avalonia;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
	DetectorModel? Model { get; set; }
	DetectorScreenshot? SelectedScreenshot { get; set; }
	ItemClass? SelectedItemClass { get; set; }
	void RemoveItem(DetectorItem item);
	void MarkAsAssets(IReadOnlyCollection<int> screenshotsIndexes);
	void RemoveScreenshots(IReadOnlyCollection<int> screenshotsIndexes);
	bool BeginDrawing(Point position);
	void UpdateDrawing(Point position);
	void EndDrawing(Point position);
}