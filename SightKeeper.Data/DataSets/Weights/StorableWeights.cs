using FlakeId;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Weights;

public interface StorableWeights : Domain.DataSets.Weights.Weights
{
	Id Id { get; }
	new IReadOnlyList<StorableTag> Tags { get; }
	
	void DeleteData();
	IReadOnlyList<Tag> Domain.DataSets.Weights.Weights.Tags => Tags;
}