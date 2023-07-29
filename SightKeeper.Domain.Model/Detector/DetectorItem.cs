using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorItem
{
	public DetectorAsset Asset { get; private set; }
	public ItemClass ItemClass { get; set; }
	public BoundingBox BoundingBox { get; set; }
	
	internal DetectorItem(DetectorAsset asset, ItemClass itemClass, BoundingBox boundingBox)
	{
		Asset = asset;
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}
	
	private DetectorItem()
	{
		Asset = null!;
		ItemClass = null!;
		BoundingBox = null!;
	}
}