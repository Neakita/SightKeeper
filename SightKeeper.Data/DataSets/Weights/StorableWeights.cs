using FlakeId;

namespace SightKeeper.Data.DataSets.Weights;

public interface StorableWeights : Domain.DataSets.Weights.Weights
{
	Id Id { get; }
}