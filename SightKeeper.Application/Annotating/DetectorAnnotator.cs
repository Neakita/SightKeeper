using Avalonia;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
	DetectorModel? Model { get; set; }
	DetectorScreenshot? SelectedScreenshot { get; set; }
	ItemClass? SelectedItemClass { get; set; }

	void AddItem(Point position, Size size, Size canvasSize);
	void RemoveItem(DetectorItem item);
	void Move(DetectorItem item, Point position, Size size);
	void MarkAsAsset(DetectorScreenshot screenshot);
	void MarkAsAsset(int screenshotIndex)
	{
		if (Model == null) throw new NullReferenceException("Detector model is null");
		MarkAsAsset(Model.DetectorScreenshots[screenshotIndex]);
	}

	void RemoveScreenshot(DetectorScreenshot screenshot);
	void RemoveScreenshots(IReadOnlyCollection<DetectorScreenshot> screenshots);
}