using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorItem
{
	public ItemClass ItemClass { get; set; }
	public BoundingBox BoundingBox { get; set; }
	
	internal DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}
	
	private DetectorItem()
	{
		ItemClass = null!;
		BoundingBox = null!;
	}
}