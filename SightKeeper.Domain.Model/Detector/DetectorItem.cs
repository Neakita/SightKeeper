using ReactiveUI;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public class DetectorItem : ReactiveObject, Entity
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

	public int Id { get; private set; }
	public virtual ItemClass ItemClass { get; private set; }
	public virtual BoundingBox BoundingBox { get; private set; }
}