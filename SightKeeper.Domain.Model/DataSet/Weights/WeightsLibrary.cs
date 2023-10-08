using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class WeightsLibrary
{
    public Id Id { get; private set; }
    public DataSet DataSet { get; private set; }
    public IReadOnlyCollection<Weights> Weights => _weights;

    internal WeightsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _weights = new List<Weights>();
    }

    public Weights CreateWeights(
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<Asset> assets)
    {
        Weights weights = new(this, onnxData, ptData, size, epoch, boundingLoss, classificationLoss, deformationLoss, assets);
        _weights.Add(weights);
        return weights;
    }

    public void RemoveWeights(Weights weights) => Guard.IsTrue(_weights.Remove(weights));

    private readonly List<Weights> _weights;

    private WeightsLibrary()
    {
        DataSet = null!;
        _weights = null!;
    }
}