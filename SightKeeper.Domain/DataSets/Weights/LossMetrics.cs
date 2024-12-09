namespace SightKeeper.Domain.DataSets.Weights;

public readonly struct LossMetrics : IEquatable<LossMetrics>
{
	public static bool operator ==(LossMetrics left, LossMetrics right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(LossMetrics left, LossMetrics right)
	{
		return !left.Equals(right);
	}

	public float BoundingLoss { get; }
	public float ClassificationLoss { get; }
	public float DeformationLoss { get; }

	public LossMetrics(float boundingLoss, float classificationLoss, float deformationLoss)
	{
		BoundingLoss = boundingLoss;
		ClassificationLoss = classificationLoss;
		DeformationLoss = deformationLoss;
	}
	public bool Equals(LossMetrics other)
	{
		return BoundingLoss.Equals(other.BoundingLoss) &&
		       ClassificationLoss.Equals(other.ClassificationLoss) &&
		       DeformationLoss.Equals(other.DeformationLoss);
	}
	public override bool Equals(object? obj)
	{
		return obj is LossMetrics other && Equals(other);
	}
	public override int GetHashCode()
	{
		return HashCode.Combine(BoundingLoss, ClassificationLoss, DeformationLoss);
	}

	public override string ToString()
	{
		return $"{nameof(BoundingLoss)}: {BoundingLoss}, {nameof(ClassificationLoss)}: {ClassificationLoss}, {nameof(DeformationLoss)}: {DeformationLoss}";
	}
}