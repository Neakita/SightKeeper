namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPoint
{
	public Vector2<double> Position { get; set; }

	internal KeyPoint(Vector2<double> position)
	{
		Position = position;
	}
}