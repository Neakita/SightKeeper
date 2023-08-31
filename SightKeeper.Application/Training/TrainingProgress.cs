namespace SightKeeper.Application.Training;

public sealed class TrainingProgress
{
    public uint CurrentEpoch { get; }
    public float BoundingLoss { get; }
    public float ClassificationLoss { get; }
    public float DeformationLoss { get; }

    public TrainingProgress(uint currentEpoch, float boundingLoss, float classificationLoss, float deformationLoss)
    {
        CurrentEpoch = currentEpoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        DeformationLoss = deformationLoss;
    }

    public override string ToString() => $"{nameof(CurrentEpoch)}: {CurrentEpoch}, {nameof(BoundingLoss)}: {BoundingLoss}, {nameof(ClassificationLoss)}: {ClassificationLoss}, {nameof(DeformationLoss)}: {DeformationLoss}";
}