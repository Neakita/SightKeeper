namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class KeyPointPositionConstraintException : Exception
{
	public KeyPoint KeyPoint { get; }
	public Vector2<double> Value { get; }

	public KeyPointPositionConstraintException(string? message, KeyPoint keyPoint, Vector2<double> value) : base(message)
	{
		KeyPoint = keyPoint;
		Value = value;
	}
}