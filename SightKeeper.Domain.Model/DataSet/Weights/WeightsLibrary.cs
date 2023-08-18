using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

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
        DateTime trainedDate,
        ModelConfig config,
        int epoch,
        float boundingLoss,
        float classificationLoss,
        IEnumerable<Asset> assets)
    {
        var assetsList = assets.ToList();
        if (DataSet is DetectorDataSet detectorModel)
        {
            if (assetsList.Any(asset => asset is not DetectorAsset detectorAsset || detectorAsset.DataSet != detectorModel))
                ThrowHelper.ThrowArgumentException(nameof(assets), $"Some assets do not belong to the \"{detectorModel}\" model");
        }
        else
            ThrowHelper.ThrowNotSupportedException("Validation of assets for the classifier model is not implemented");
        Weights weights = new(data, trainedDate, config, this, epoch, boundingLoss, classificationLoss, assetsList);
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