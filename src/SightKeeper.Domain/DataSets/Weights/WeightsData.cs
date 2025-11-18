using FlakeId;

namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsData
{
	Id Id { get; }
	WeightsMetadata Metadata { get; }
}