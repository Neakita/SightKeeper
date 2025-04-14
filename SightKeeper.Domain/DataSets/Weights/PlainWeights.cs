using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PlainWeights : Weights
{
	public required IReadOnlyCollection<Tag> Tags { get; init; }
}