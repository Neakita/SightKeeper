using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public class DetectorItem : ReactiveObject, Entity
{
	public DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		ItemClass = itemClass;
		BoundingBox = boundingBox;
		// EF sets the screenshot property itself when adding an item to an observable collection of screenshot items
		Screenshot = null!;
	}
	
	private DetectorItem()
	{
		ItemClass = null!;
		BoundingBox = null!;
		Screenshot = null!;
	}

	public int Id { get; private set; } = -1;
	[Reactive] public ItemClass ItemClass { get; set; }
	[Reactive] public BoundingBox BoundingBox { get; set; }
	public int ScreenshotId { get; private set; } = -1;
	public DetectorScreenshot Screenshot { get; private set; }
}