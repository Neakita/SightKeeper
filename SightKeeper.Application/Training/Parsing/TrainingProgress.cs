namespace SightKeeper.Application.Training.Parsing;

public struct TrainingProgress
{
    public int CurrentBatch;
    public double AverageLoss;

    public TrainingProgress(int currentBatch, double averageLoss)
    {
        CurrentBatch = currentBatch;
        AverageLoss = averageLoss;
    }
}