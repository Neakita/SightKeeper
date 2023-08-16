using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class InternalTrainedWeights : Weights
{
    public int Batch { get; private set; }
    public float AverageLoss { get; private set; }
    public float? Accuracy { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }
    
    public InternalTrainedWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        WeightsLibrary library,
        int batch,
        float averageLoss,
        float? accuracy,
        IEnumerable<Asset> assets)
        : base(data, trainedDate, config, library)
    {
        Batch = batch;
        AverageLoss = averageLoss;
        Accuracy = accuracy;
        Assets = assets.ToList();
    }

    private InternalTrainedWeights()
    {
        Assets = null!;
    }
}