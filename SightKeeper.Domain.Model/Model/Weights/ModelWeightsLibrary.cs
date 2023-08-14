using SightKeeper.Domain.Model.Common;

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
        ModelWeightsLibrary library,
        int batch,
        float averageLoss,
        float? accuracy,
        IEnumerable<Asset> assets)
    {
        InternalTrainedModelWeights weights = new(data, trainedDate, config, library, batch, averageLoss, accuracy, assets);
        _weights.Add(weights);
        return weights;
    }

    public PreTrainedModelWeights CreateWeights(
        byte[] data,
        DateTime trainedDate,
        ModelConfig config,
        ModelWeightsLibrary library,
        DateTime addedDate)
    {
        PreTrainedModelWeights weights = new(data, trainedDate, config, library, addedDate);
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