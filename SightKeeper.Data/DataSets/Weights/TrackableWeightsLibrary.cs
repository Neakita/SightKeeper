using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(StorableWeightsLibrary inner, ChangeListener listener) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => inner.Weights;

	public void CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		inner.CreateWeights(metadata, tags);
		listener.SetDataChanged();
	}

	public void RemoveWeights(StorableWeights weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}
}