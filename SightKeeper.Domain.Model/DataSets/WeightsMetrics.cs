namespace SightKeeper.Domain.Model.DataSets;

public readonly struct WeightsMetrics
{
	public static bool operator ==(WeightsMetrics left, WeightsMetrics right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(WeightsMetrics left, WeightsMetrics right)
	{
		return !(left == right);
	}

	public uint Epoch { get; }
	public LossMetrics LossMetrics { get; }

	public WeightsMetrics()
	{
	}
	public WeightsMetrics(uint epoch, LossMetrics lossMetrics)
	{
		Epoch = epoch;
		LossMetrics = lossMetrics;
	}
	public bool Equals(WeightsMetrics other)
	{
		return Epoch == other.Epoch && LossMetrics == other.LossMetrics;
	}
	public override bool Equals(object? obj)
	{
		return obj is WeightsMetrics other && Equals(other);
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(Epoch, LossMetrics);
	}
	public override string ToString() => $"{nameof(Epoch)}: {Epoch}, {LossMetrics}";
}