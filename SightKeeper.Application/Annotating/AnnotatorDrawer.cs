using Avalonia;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface AnnotatorDrawer
{
	event Action<DetectorItem> Drawn;

	DetectorScreenshot? Screenshot { get; set; }
	ItemClass? ItemClass { get; set; }
	void BeginDrawing(Point startPosition);
	void UpdateDrawing(Point currentPosition);
	void EndDrawing(Point finishPosition);
}