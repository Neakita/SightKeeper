using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorItem
{
	public DetectorAsset Asset { get; private set; }
	public ItemClass ItemClass { get; set; }
	public BoundingBox Bounding { get; private set; }
	
	internal DetectorItem(DetectorAsset asset, ItemClass itemClass, BoundingBox bounding)
	{
		Asset = asset;
		ItemClass = itemClass;
		Bounding = bounding;
	}
	
	private DetectorItem()
	{
		Asset = null!;
		ItemClass = null!;
		Bounding = null!;
	}
}