using FlakeId;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeights(Id id, WeightsMetadata metadata) : WeightsData
{
	public Id Id => id;
	public WeightsMetadata Metadata => metadata;
}