using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class Weights : ObservableObject
{
    public Id Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public WeightsLibrary Library { get; private set; }
    public ONNXData ONNXData { get; private set; }
    public PTData PTData { get; private set; }
    public ModelSize Size { get; private set; }
    public WeightsMetrics Metrics { get; private set; }
    public IReadOnlyList<ItemClass> ItemClasses { get; private set; }

    internal Weights(
        WeightsLibrary library,
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<ItemClass> itemClasses)
    {
        CreationDate = DateTime.Now;
        Library = library;
        ONNXData = new ONNXData(onnxData);
        PTData = new PTData(ptData);
        Size = size;
        Metrics = metrics;
        ItemClasses = itemClasses.ToImmutableList();
        Guard.IsTrue(ItemClasses.All(itemClass => itemClass.DataSet == library.DataSet));
    }

    private Weights()
    {
        Library = null!;
        ONNXData = null!;
        PTData = null!;
        ItemClasses = null!;
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {nameof(Metrics.Epoch)}: {Metrics.Epoch}, {nameof(Metrics.BoundingLoss)}: {Metrics.BoundingLoss}, {nameof(Metrics.ClassificationLoss)}: {Metrics.ClassificationLoss}, {nameof(Metrics.DeformationLoss)}: {Metrics.DeformationLoss}";
}