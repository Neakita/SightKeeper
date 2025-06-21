using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Model.DataSets.Weights;

internal sealed class InMemoryWeightsLibrary : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => _weights;

	public void AddWeights(Domain.DataSets.Weights.Weights weights)
	{
		throw new NotImplementedException();
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		bool isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	internal void EnsureCapacity(int capacity)
	{
		_weights.EnsureCapacity(capacity);
	}

	private readonly List<Domain.DataSets.Weights.Weights> _weights = new();
}