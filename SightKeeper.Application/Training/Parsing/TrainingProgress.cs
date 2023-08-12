namespace SightKeeper.Application.Training.Parsing;

public readonly struct TrainingProgress
{
    public uint? Batch { get; }
    public float? Accuracy { get; }

    public TrainingProgress(uint? batch, float? accuracy)
    {
        Batch = batch;
        Accuracy = accuracy;
    }

    public override string ToString() => $"CurrentBatch: {Batch}, AverageLoss: {Accuracy}";
}