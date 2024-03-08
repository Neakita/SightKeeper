using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Weights
{
    public DateTime CreationDate { get; }
    public WeightsData ONNXWeightsData { get; }
    public WeightsData PTWeightsData { get; }
    public Size Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public ImmutableList<ItemClass> ItemClasses { get; }

    internal Weights(
        byte[] onnxData,
        byte[] ptData,
        Size size,
        WeightsMetrics weightsMetrics,
        IEnumerable<ItemClass> itemClasses)
    {
        CreationDate = DateTime.Now;
        ONNXWeightsData = new WeightsData(onnxData);
        PTWeightsData = new WeightsData(ptData);
        Size = size;
        WeightsMetrics = weightsMetrics;
        ItemClasses = itemClasses.ToImmutableList();
    }

    private Weights()
    {
        ONNXWeightsData = null!;
        PTWeightsData = null!;
        ItemClasses = null!;
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {WeightsMetrics}";
}