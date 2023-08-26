using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Weights
{
    public byte[] Data { get; private set; }
    public ModelSize Size { get; private set; }
    public uint Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public float DeformationLoss { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }

    public Weights(byte[] data, ModelSize size, uint epoch, float boundingLoss, float classificationLoss, float deformationLoss, IEnumerable<Asset> assets)
    {
        Data = data;
        Size = size;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        DeformationLoss = deformationLoss;
        Assets = assets.ToList();
    }

    private Weights()
    {
        Data = null!;
        Assets = null!;
    }
}