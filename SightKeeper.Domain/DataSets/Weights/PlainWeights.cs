using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PlainWeights : Weights
{
	public IReadOnlyCollection<Tag> Tags { get; }

	internal PlainWeights(
		DateTimeOffset creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IReadOnlyCollection<Tag> tags)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}