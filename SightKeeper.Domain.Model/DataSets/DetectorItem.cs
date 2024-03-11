namespace SightKeeper.Domain.Model.DataSets;

public sealed class DetectorItem
{
	public ItemClass ItemClass { get; set; }
	public Bounding Bounding { get; set; }
	public Asset Asset { get; }
	
	internal DetectorItem(ItemClass itemClass, Bounding bounding, Asset asset)
	{
		ItemClass = itemClass;
		Bounding = bounding;
		Asset = asset;
	}
}