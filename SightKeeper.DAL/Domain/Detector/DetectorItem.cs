using System.ComponentModel.DataAnnotations;
using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Detector;

public class DetectorItem
{
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

	[Key] public int Id { get; }
	public virtual ItemClass ItemClass { get; }
	public virtual BoundingBox BoundingBox { get; }
}