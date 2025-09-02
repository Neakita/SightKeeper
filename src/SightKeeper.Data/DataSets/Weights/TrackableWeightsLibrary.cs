using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class TrackableWeightsLibrary(StorableWeightsLibrary inner, ChangeListener listener) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => inner.Weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<StorableTag> tags)
	{
		var weights = inner.CreateWeights(metadata, tags);
		listener.SetDataChanged();
		return weights;
	}

	public void RemoveWeights(StorableWeights weights)
	{
		inner.RemoveWeights(weights);
		listener.SetDataChanged();
	}

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}

	public void AddWeights(StorableWeights weights)
	{
		inner.AddWeights(weights);
	}
}