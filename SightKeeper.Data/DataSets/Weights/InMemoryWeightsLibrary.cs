using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeightsLibrary(WeightsWrapper weightsWrapper) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => _weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<StorableTag> tags)
	{
		var id = Id.Create();
		var inMemoryWeights = new InMemoryWeights(id, metadata, tags.ToList());
		var wrappedWeights = weightsWrapper.Wrap(inMemoryWeights);
		_weights.Add(wrappedWeights);
		return wrappedWeights;
	}

	public void RemoveWeights(StorableWeights weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	internal void EnsureCapacity(int capacity)
	{
		_weights.EnsureCapacity(capacity);
	}

	internal void AddWeights(InMemoryWeights weights)
	{
		var wrappedWeights = weightsWrapper.Wrap(weights);
		_weights.Add(wrappedWeights);
	}

	private readonly List<StorableWeights> _weights = new();
}