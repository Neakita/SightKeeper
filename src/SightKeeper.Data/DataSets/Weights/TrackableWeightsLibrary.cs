using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(WeightsLibrary inner, ChangeListener listener) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => inner.Weights;

	public Domain.DataSets.Weights.Weights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		listener.SetDataChanged();
		return weights;
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}
}