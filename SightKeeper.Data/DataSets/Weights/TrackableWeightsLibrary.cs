using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(WeightsLibrary inner, ChangeListener listener) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => inner.Weights;

	public void CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		inner.CreateWeights(metadata, tags);
		listener.SetDataChanged();
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}
}