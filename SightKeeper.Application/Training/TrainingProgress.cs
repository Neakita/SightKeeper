namespace SightKeeper.Application.Training;

public readonly struct TrainingProgress
{
    public uint Batch { get; }
    public float AverageLoss { get; }

    public TrainingProgress(uint batch, float averageLoss)
    {
        Batch = batch;
        AverageLoss = averageLoss;
    }

    public override string ToString() => $"CurrentBatch: {Batch}\nAverageLoss: {AverageLoss}";
}