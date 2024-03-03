using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles;

public readonly struct PreemptionStabilizationSettings
{
	public static bool operator ==(PreemptionStabilizationSettings left, PreemptionStabilizationSettings right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(PreemptionStabilizationSettings left, PreemptionStabilizationSettings right)
	{
		return !left.Equals(right);
	}

	public byte BufferSize { get; }
	public PreemptionStabilizationMethod Method { get; }

	public PreemptionStabilizationSettings(byte bufferSize, PreemptionStabilizationMethod method)
	{
		Guard.IsGreaterThanOrEqualTo((int)bufferSize, 2);
		BufferSize = bufferSize;
		Method = method;
	}

	public bool Equals(PreemptionStabilizationSettings other)
	{
		return BufferSize == other.BufferSize && Method == other.Method;
	}

	public override bool Equals(object? obj)
	{
		return obj is PreemptionStabilizationSettings other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(BufferSize, (int)Method);
	}
}