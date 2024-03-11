using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles;

public readonly struct PreemptionSettings
{
	public static bool operator ==(PreemptionSettings left, PreemptionSettings right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(PreemptionSettings left, PreemptionSettings right)
	{
		return !left.Equals(right);
	}

	public Vector2<float> Factor { get; }
	public StabilizationSettings? StabilizationSettings { get; }

	public PreemptionSettings(Vector2<float> factor)
	{
		Guard.IsGreaterThanOrEqualTo(factor.X, 0);
		Guard.IsGreaterThanOrEqualTo(factor.Y, 0);
		Factor = factor;
	}

	public PreemptionSettings(Vector2<float> factor, StabilizationSettings stabilizationSettings) : this(factor)
	{
		StabilizationSettings = stabilizationSettings;
	}

	public bool Equals(PreemptionSettings other)
	{
		return Factor.Equals(other.Factor) && Nullable.Equals(StabilizationSettings, other.StabilizationSettings);
	}

	public override bool Equals(object? obj)
	{
		return obj is PreemptionSettings other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Factor, StabilizationSettings);
	}
}