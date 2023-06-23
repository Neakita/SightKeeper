using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public class DetectorItem : Entity
{
	public ItemClass ItemClass { get; set; }
	public BoundingBox BoundingBox { get; set; }
	
	public DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
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