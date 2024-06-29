namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPoint
{
	public Vector2<double> Position { get; set; }
	public KeyPointTag Tag { get; }

	internal KeyPoint(Vector2<double> position, KeyPointTag tag)
	{
		Position = position;
		Tag = tag;
	}
}