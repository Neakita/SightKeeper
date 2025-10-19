using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeightsLibrary(Wrapper<WeightsData> weightsWrapper) : WeightsLibrary
{
	public IReadOnlyCollection<WeightsData> Weights => _weights;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		var id = Id.Create();
		var inMemoryWeights = new InMemoryWeights(id, metadata, tags.ToList());
		var wrappedWeights = weightsWrapper.Wrap(inMemoryWeights);
		_weights.Add(wrappedWeights);
		return wrappedWeights;
	}

	public void RemoveWeights(WeightsData weights)
	{
		var isRemoved = _weights.Remove(weights);
		Guard.IsTrue(isRemoved);
	}

	public void AddWeights(WeightsData weights)
	{
		var wrappedWeights = weightsWrapper.Wrap(weights);
		_weights.Add(wrappedWeights);
	}

	private readonly List<WeightsData> _weights = new();
}