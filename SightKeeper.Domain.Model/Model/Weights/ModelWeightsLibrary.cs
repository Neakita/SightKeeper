using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model;

public sealed class ModelWeightsLibrary
{
    public Model Model { get; private set; }
    public IReadOnlyCollection<ModelWeights> Weights => _weights;

    internal ModelWeightsLibrary(Model model)
    {
        Model = model;
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
        if (Model is DetectorModel detectorModel)
        {
            if (assetsList.Any(asset => asset is not DetectorAsset detectorAsset || detectorAsset.Model != detectorModel))
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
        Model = null!;
        _weights = null!;
    }
}