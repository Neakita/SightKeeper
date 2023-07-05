using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.Common;

public sealed class ModelWeightsLibrary
{
    public IReadOnlyCollection<ModelWeights> Weights => _weights;

    internal ModelWeightsLibrary()
    {
        _weights = new List<ModelWeights>();
    }
    
    public void AddWeights(ModelWeights weights)
    {
        if (_weights.Contains(weights)) ThrowHelper.ThrowArgumentException("Weights already added");
        _weights.Add(weights);
    }
	
    private readonly List<ModelWeights> _weights;
}