using System.Diagnostics.CodeAnalysis;

namespace SightKeeper.Domain.DataSets.Poser;

public sealed class KeyPointPositionConstraintException : Exception
{
	public static void ThrowIfNotNormalized(KeyPoint keyPoint, Vector2<double> value)
	{
		if (!IsNormalized(value))
			ThrowForNotNormalized(keyPoint, value);
	}

	[DoesNotReturn]
	private static void ThrowForNotNormalized(KeyPoint keyPoint, Vector2<double> value)
	{
		const string? message =
			"Both aspects of the position vector must be normalized, i.e. be in the inclusive range from 0 to 1";
		throw new KeyPointPositionConstraintException(message, keyPoint, value);
	}

	public KeyPoint KeyPoint { get; }
	public Vector2<double> Value { get; }

	public KeyPointPositionConstraintException(string? message, KeyPoint keyPoint, Vector2<double> value) : base(message)
	{
		KeyPoint = keyPoint;
		Value = value;
	}

	private static bool IsNormalized(Vector2<double> vector)
	{
		return IsNormalized(vector.X) && IsNormalized(vector.Y);
	}

	private static bool IsNormalized(double value)
	{
		return value is >= 0 and <= 1;
	}
}