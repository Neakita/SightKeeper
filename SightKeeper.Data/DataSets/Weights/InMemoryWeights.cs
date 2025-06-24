using FlakeId;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeights : Domain.DataSets.Weights.Weights
{
	public Id Id { get; }
	public WeightsMetadata Metadata { get; }
	public IReadOnlyList<Tag> Tags { get; }

	public InMemoryWeights(Id id, WeightsMetadata metadata, IReadOnlyList<Tag> tags)
	{
		Id = id;
		Metadata = metadata;
		Tags = tags;
	}

	public Stream? OpenWriteSteam()
	{
		return null;
	}

	public Stream? OpenReadStream()
	{
		return null;
	}
}