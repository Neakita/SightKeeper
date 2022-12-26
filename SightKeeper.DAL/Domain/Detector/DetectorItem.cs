using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Detector;

public class DetectorItem
{
	[Key] public int Id { get; private set; }
	public virtual ItemClass ItemClass { get; private set; }
	public virtual BoundingBox BoundingBox { get; private set; }
	
	
	public DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}


	private DetectorItem()
	{
		Id = 0;
		ItemClass = null!;
		BoundingBox = null!;
	}
}