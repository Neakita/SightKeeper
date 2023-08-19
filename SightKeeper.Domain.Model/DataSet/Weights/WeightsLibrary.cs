using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public abstract class WeightsLibrary
{
}

public sealed class WeightsLibrary<TAsset> : WeightsLibrary
    where TAsset : Asset
{
    public IReadOnlyCollection<Weights<TAsset>> Weights => _weights;

    internal WeightsLibrary()
    {
        _weights = new List<Weights<TAsset>>();
    }

    public Weights<TAsset> CreateWeights(
        byte[] data,
        DateTime trainedDate,
        ModelSize size,
        uint epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<TAsset> assets)
    {
        var assetsList = assets.ToList();
        Weights<TAsset> weights = new(data, trainedDate, size, epoch, boundingLoss, classificationLoss, assetsList);
        _weights.Add(weights);
        return weights;
    }
	
    private readonly List<Weights<TAsset>> _weights;
}