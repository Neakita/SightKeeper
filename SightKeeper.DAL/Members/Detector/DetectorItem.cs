using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Detector;

public class DetectorItem
{
	public DetectorScreenshot Screenshot { get; private set; }
	public ItemClass ItemClass { get; private set; }
	public BoundingBox BoundingBox { get; private set; }
	
	
	public DetectorItem(DetectorScreenshot screenshot, ItemClass itemClass, BoundingBox boundingBox)
	{
		Screenshot = screenshot;
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}


	private DetectorItem()
	{
		Screenshot = null!;
		ItemClass = null!;
		BoundingBox = null!;
	}
}