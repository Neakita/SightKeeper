namespace SightKeeper.Backend.Data.Members.Detector;

public class DetectorItem
{
	public Guid Id { get; set; }
	public ItemClass ItemClass { get; set; }
	public BoundingBox BoundingBox { get; set; }
}