using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public sealed class ModelWeightsLibrary
{
    public DataSet DataSet { get; private set; }
    public IReadOnlyCollection<ModelWeights> Weights => _weights;

    internal ModelWeightsLibrary(DataSet dataSet)
    {
        DataSet = dataSet;
        _weights = new List<ModelWeights>();
    }

    public InternalTrainedModelWeights CreateWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        int batch,
        float averageLoss,
        float? accuracy,
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
        InternalTrainedModelWeights weights = new(data, trainedDate, config, this, batch, averageLoss, accuracy, assetsList);
        _weights.Add(weights);
        return weights;
    }

    public PreTrainedModelWeights CreateWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        DateTime addedDate)
    {
        PreTrainedModelWeights weights = new(data, trainedDate, config, this, addedDate);
        _weights.Add(weights);
        return weights;
    }
	
    private readonly List<ModelWeights> _weights;

    private ModelWeightsLibrary()
    {
        DataSet = null!;
        _weights = null!;
    }
}