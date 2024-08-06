namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class KeyPoint2D
{
	public Vector2<double> Position { get; set; }
	public KeyPointTag2D Tag { get; }

	internal KeyPoint2D(Vector2<double> position, KeyPointTag2D tag)
	{
		Position = position;
		Tag = tag;
	}
}