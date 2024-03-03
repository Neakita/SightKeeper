namespace SightKeeper.Domain.Model;

public readonly struct WeightsMetrics
{
	public uint Epoch { get; }
	public float BoundingLoss { get; }
	public float ClassificationLoss { get; }
	public float DeformationLoss { get; }

	public WeightsMetrics(uint epoch, float boundingLoss, float classificationLoss, float deformationLoss)
	{
		Epoch = epoch;
		BoundingLoss = boundingLoss;
		ClassificationLoss = classificationLoss;
		DeformationLoss = deformationLoss;
	}
	
	public bool Equals(WeightsMetrics other)
	{
		return Epoch == other.Epoch && BoundingLoss.Equals(other.BoundingLoss) && ClassificationLoss.Equals(other.ClassificationLoss) && DeformationLoss.Equals(other.DeformationLoss);
	}

	public override bool Equals(object? obj)
	{
		return obj is WeightsMetrics other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Epoch, BoundingLoss, ClassificationLoss, DeformationLoss);
	}
	
	public override string ToString() => $"{nameof(Epoch)}: {Epoch}, {nameof(BoundingLoss)}: {BoundingLoss}, {nameof(ClassificationLoss)}: {ClassificationLoss}, {nameof(DeformationLoss)}: {DeformationLoss}";
}