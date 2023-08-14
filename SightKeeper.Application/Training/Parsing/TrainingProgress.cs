namespace SightKeeper.Application.Training.Parsing;

public readonly struct TrainingProgress
{
    public uint Batch { get; }
    public float AverageLoss { get; }

    public TrainingProgress(uint batch, float averageLoss)
    {
        Batch = batch;
        AverageLoss = averageLoss;
    }

    public override string ToString() => $"CurrentBatch: {Batch}, AverageLoss: {AverageLoss}";
}