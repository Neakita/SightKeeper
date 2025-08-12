using FlakeId;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeights : StorableWeights
{
	public Id Id { get; }
	public WeightsMetadata Metadata { get; }
	public IReadOnlyList<StorableTag> Tags { get; }

	public InMemoryWeights(Id id, WeightsMetadata metadata, IReadOnlyList<StorableTag> tags)
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

	public void DeleteData()
	{
	}
}