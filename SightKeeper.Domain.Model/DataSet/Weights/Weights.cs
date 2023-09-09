using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Weights
{
    public WeightsLibrary Library { get; private set; }
    public byte[] ONNXData { get; private set; }
    public byte[] PTData { get; private set; }
    public ModelSize Size { get; private set; }
    public uint Epoch { get; private set; }
    public float BoundingLoss { get; private set; }
    public float ClassificationLoss { get; private set; }
    public float DeformationLoss { get; private set; }
    public IReadOnlyCollection<Asset> Assets { get; private set; }

    internal Weights(WeightsLibrary library, byte[] onnxData, byte[] ptData, ModelSize size, uint epoch, float boundingLoss, float classificationLoss, float deformationLoss, IEnumerable<Asset> assets)
    {
        Library = library;
        ONNXData = onnxData;
        PTData = ptData;
        Size = size;
        Epoch = epoch;
        BoundingLoss = boundingLoss;
        ClassificationLoss = classificationLoss;
        DeformationLoss = deformationLoss;
        Assets = assets.ToList();
    }

    private Weights()
    {
        Library = null!;
        ONNXData = null!;
        PTData = null!;
        Assets = null!;
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {nameof(Epoch)}: {Epoch}, {nameof(BoundingLoss)}: {BoundingLoss}, {nameof(ClassificationLoss)}: {ClassificationLoss}, {nameof(DeformationLoss)}: {DeformationLoss}";
}