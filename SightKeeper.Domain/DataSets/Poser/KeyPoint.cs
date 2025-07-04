using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Poser;

public class KeyPoint : TagUser
{
	public Tag Tag { get; }

	public Vector2<double> Position
	{
		get;
		set
		{
			if (!IsDimensionsNormalized(value))
			{
				const string? keyPointPositionConstraintExceptionMessage =
					"Both aspects of the position vector must be normalized, i.e. be in the inclusive range from 0 to 1";
				throw new KeyPointPositionConstraintException(keyPointPositionConstraintExceptionMessage, this, value);
			}
			field = value;
		}
	}

	internal KeyPoint(Tag tag)
	{
		Tag = tag;
	}

	private static bool IsDimensionsNormalized(Vector2<double> vector)
	{
		return IsNormalized(vector.X) && IsNormalized(vector.Y);
	}

	private static bool IsNormalized(double value)
	{
		return value is >= 0 and <= 1;
	}
}