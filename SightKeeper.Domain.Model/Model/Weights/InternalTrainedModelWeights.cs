using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class InternalTrainedModelWeights : ModelWeights
{
    public int Batch { get; private set; }
    public float Accuracy { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }
    
    public InternalTrainedModelWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        ModelWeightsLibrary library,
        int batch,
        float accuracy,
        IEnumerable<Asset> assets)
        : base(data, trainedDate, config, library)
    {
        Batch = batch;
        Accuracy = accuracy;
        Assets = assets.ToList();
    }

    private InternalTrainedModelWeights()
    {
        Assets = null!;
    }
}