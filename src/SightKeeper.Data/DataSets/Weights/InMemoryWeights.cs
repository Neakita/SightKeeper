using FlakeId;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class InMemoryWeights(Id id, WeightsMetadata metadata) : WeightsData, IdHolder
{
	public Id Id => id;
	public WeightsMetadata Metadata => metadata;
}