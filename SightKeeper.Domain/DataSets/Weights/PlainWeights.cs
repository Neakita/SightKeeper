using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PlainWeights : Weights
{
	public IReadOnlyCollection<Tag> Tags { get; }

	internal PlainWeights(
		DateTimeOffset creationTimestamp,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IReadOnlyCollection<Tag> tags)
		: base(creationTimestamp, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}