namespace SightKeeper.Domain.Model;

public sealed class WeightsLibrary
{
    public IReadOnlyCollection<Weights> Weights => _weights;

    internal WeightsLibrary()
    {
        _weights = new List<Weights>();
    }

    public Weights CreateWeights(
        byte[] data,
        ModelSize size,
        uint epoch,
        float loss)
    {
        Weights weights = new(data, size, epoch, loss);
        _weights.Add(weights);
        return weights;
    }
	
    private readonly List<Weights> _weights;
}