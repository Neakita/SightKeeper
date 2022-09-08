namespace SightKeeper.Backend.Data.Members.Detector;

public sealed class BoundingBox
{
	public Guid Id { get; set; }
	public float X { get; set; }
	public float Y { get; set; }
	public float Width { get; set; }
	public float Height { get; set; }
}
