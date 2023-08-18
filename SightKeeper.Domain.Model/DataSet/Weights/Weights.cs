using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Weights
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelSize Size { get; private set; }
    public WeightsLibrary Library { get; private set; }
    public uint Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }
    
    internal Weights(byte[] data, DateTime trainedDate, ModelSize size, WeightsLibrary library, uint epoch, float boundingLoss, float classificationLoss, IReadOnlyCollection<Asset> assets)
    {
        Data = data;
        TrainedDate = trainedDate;
        Size = size;
        Library = library;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        Assets = assets;
    }

    private Weights()
    {
        Data = null!;
        Library = null!;
        Assets = null!;
    }
}