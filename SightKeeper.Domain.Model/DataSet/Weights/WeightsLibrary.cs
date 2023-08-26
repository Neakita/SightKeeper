using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class WeightsLibrary
{
    public DataSet DataSet { get; private set; }
    public IReadOnlyCollection<Weights> Weights => _weights;

    internal WeightsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _weights = new List<Weights>();
    }

    public Weights CreateWeights(
        byte[] data,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        float deformationLoss,
        IEnumerable<Asset> assets)
    {
        Weights weights = new(data, size, epoch, boundingLoss, classificationLoss, deformationLoss, assets);
        _weights.Add(weights);
        return weights;
    }
	
    private readonly List<Weights> _weights;

    private WeightsLibrary()
    {
        DataSet = null!;
        _weights = null!;
    }
}