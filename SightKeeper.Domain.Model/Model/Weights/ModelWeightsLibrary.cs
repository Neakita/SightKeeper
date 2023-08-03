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

    public ModelWeights CreateWeights(int batch, byte[] data, IEnumerable<Asset> assets, ModelConfig? config = null)
    {
        ModelWeights weights = new(Model, batch, data, assets, config);
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