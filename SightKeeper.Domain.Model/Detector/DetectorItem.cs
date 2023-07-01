using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorItem
{
	public ItemClass ItemClass { get; set; }
	public BoundingBox BoundingBox { get; set; }
	
	internal DetectorItem(DetectorAsset asset, ItemClass itemClass, BoundingBox boundingBox)
	{
		_asset = asset;
		ItemClass = itemClass;
		BoundingBox = boundingBox;
	}

	private DetectorAsset _asset;
	
	private DetectorItem()
	{
		_asset = null!;
		ItemClass = null!;
		BoundingBox = null!;
	}
}