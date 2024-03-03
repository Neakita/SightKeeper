﻿using System.Collections.Immutable;

namespace SightKeeper.Domain.Model;

public sealed class Weights : Entity
{
    public DateTime CreationDate { get; }
    public WeightsData ONNXWeightsData { get; }
    public WeightsData PTWeightsData { get; }
    public ModelSize Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public ImmutableList<ItemClass> ItemClasses { get; }

    internal Weights(
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
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

    public override string ToString() => $"{nameof(Size)}: {Size}, {nameof(WeightsMetrics.Epoch)}: {WeightsMetrics.Epoch}, {nameof(WeightsMetrics.BoundingLoss)}: {WeightsMetrics.BoundingLoss}, {nameof(WeightsMetrics.ClassificationLoss)}: {WeightsMetrics.ClassificationLoss}, {nameof(WeightsMetrics.DeformationLoss)}: {WeightsMetrics.DeformationLoss}";
}