using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Profiles;

public readonly struct StabilizationSettings
{
	public static bool operator ==(StabilizationSettings left, StabilizationSettings right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(StabilizationSettings left, StabilizationSettings right)
	{
		return !left.Equals(right);
	}

	public byte BufferSize { get; }
	public StabilizationMethod Method { get; }

	public StabilizationSettings(byte bufferSize, StabilizationMethod method)
	{
		Guard.IsGreaterThanOrEqualTo((int)bufferSize, 2);
		BufferSize = bufferSize;
		Method = method;
	}

	public bool Equals(StabilizationSettings other)
	{
		return BufferSize == other.BufferSize && Method == other.Method;
	}

	public override bool Equals(object? obj)
	{
		return obj is StabilizationSettings other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(BufferSize, (int)Method);
	}
}