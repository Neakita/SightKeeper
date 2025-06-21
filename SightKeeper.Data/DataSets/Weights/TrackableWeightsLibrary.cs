using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(WeightsLibrary inner, ChangeListener listener) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => inner.Weights;

	public void AddWeights(Domain.DataSets.Weights.Weights weights)
	{
		inner.AddWeights(weights);
		listener.SetDataChanged();
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}
}