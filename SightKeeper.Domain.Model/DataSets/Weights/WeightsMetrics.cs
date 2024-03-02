namespace SightKeeper.Domain.Model.DataSets.Weights;

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
	
	public override string ToString() => $"{nameof(Epoch)}: {Epoch}, {nameof(BoundingLoss)}: {BoundingLoss}, {nameof(ClassificationLoss)}: {ClassificationLoss}, {nameof(DeformationLoss)}: {DeformationLoss}";
}