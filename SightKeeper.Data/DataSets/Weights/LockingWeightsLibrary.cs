using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Model.DataSets.Weights;

internal sealed class LockingWeightsLibrary(WeightsLibrary inner, Lock editingLock) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => inner.Weights;

	public void AddWeights(Domain.DataSets.Weights.Weights weights)
	{
		lock (editingLock)
			inner.AddWeights(weights);
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}
}