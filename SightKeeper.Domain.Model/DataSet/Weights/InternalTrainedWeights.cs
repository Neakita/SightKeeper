using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class InternalTrainedWeights : Weights
{
    public int Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }
    
    public InternalTrainedWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        WeightsLibrary library,
        int epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets)
        : base(data, trainedDate, config, library)
    {
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        Assets = assets.ToList();
    }

    private InternalTrainedWeights()
    {
        Assets = null!;
    }
}