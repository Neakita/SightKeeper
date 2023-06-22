namespace SightKeeper.Application.Training.Parsing;

public readonly struct TrainingProgress
{
    public uint? CurrentBatch { get; }
    public double? AverageLoss { get; }

    public TrainingProgress(uint? currentBatch, double? averageLoss)
    {
        CurrentBatch = currentBatch;
        AverageLoss = averageLoss;
    }

    public override string ToString() => $"CurrentBatch: {CurrentBatch}, AverageLoss: {AverageLoss}";
}