using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public abstract class Weights
{
    public byte[] Data { get; private set; }
    public DateTime TrainedDate { get; private set; }
    public ModelSize Size { get; private set; }
    public uint Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    
    protected Weights(byte[] data, DateTime trainedDate, ModelSize size, uint epoch, float boundingLoss, float classificationLoss)
    {
        Data = data;
        TrainedDate = trainedDate;
        Size = size;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
    }

    protected Weights()
    {
        Data = null!;
    }
}

public sealed class Weights<TAsset> : Weights
    where TAsset : Asset
{
    public IReadOnlyCollection<TAsset> Assets { get; private set; }
    
    internal Weights(
        byte[] data,
        DateTime trainedDate,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        IReadOnlyCollection<TAsset> assets) 
        : base(data, trainedDate, size, epoch, boundingLoss, classificationLoss)
    {
        Assets = assets;
    }

    private Weights()
    {
        Assets = null!;
    }
}