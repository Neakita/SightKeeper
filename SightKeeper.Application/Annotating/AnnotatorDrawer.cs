﻿using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using Point = Avalonia.Point;

namespace SightKeeper.Application.Annotating;

public interface AnnotatorDrawer
{
	event Action<DetectorItem> Drawn;

	DetectorScreenshot? Screenshot { get; set; }
	ItemClass? ItemClass { get; set; }
	bool BeginDrawing(Point startPosition);
	void UpdateDrawing(Point currentPosition);
	void EndDrawing(Point finishPosition);
}