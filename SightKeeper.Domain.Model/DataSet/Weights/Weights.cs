using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Weights<TAsset> where TAsset : Asset
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelSize Size { get; private set; }
    public uint Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public IReadOnlyCollection<TAsset> Assets { get; private set; }
    
    internal Weights(byte[] data, DateTime trainedDate, ModelSize size, uint epoch, float boundingLoss, float classificationLoss, IReadOnlyCollection<TAsset> assets)
    {
        Data = data;
        TrainedDate = trainedDate;
        Size = size;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        Assets = assets;
    }

    private Weights()
    {
        Data = null!;
        Assets = null!;
    }
}