using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeightsLibrary(WeightsWrapper weightsWrapper) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => _weights;

	public Domain.DataSets.Weights.Weights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var id = Id.Create();
		var inMemoryWeights = new InMemoryWeights(id, metadata, tags.ToList());
		var wrappedWeights = weightsWrapper.Wrap(inMemoryWeights);
		_weights.Add(wrappedWeights);
		return wrappedWeights;
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	public void EnsureCapacity(int capacity)
	{
		_weights.EnsureCapacity(capacity);
	}

	public void AddWeights(Domain.DataSets.Weights.Weights weights)
	{
		var wrappedWeights = weightsWrapper.Wrap(weights);
		_weights.Add(wrappedWeights);
	}

	private readonly List<Domain.DataSets.Weights.Weights> _weights = new();
}