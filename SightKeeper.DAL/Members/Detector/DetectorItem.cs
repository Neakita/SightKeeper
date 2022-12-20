using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

public class DetectorItem
{
	[Key] public Guid Id { get; private set; }
	public virtual ItemClass ItemClass { get; private set; }
	public virtual BoundingBox BoundingBox { get; private set; }
	
	
	public DetectorItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}


	private DetectorItem()
	{
		Id = Guid.Empty;
		ItemClass = null!;
		BoundingBox = null!;
	}
}