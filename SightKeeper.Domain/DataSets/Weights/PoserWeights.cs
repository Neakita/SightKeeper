using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PoserWeights : Weights
{
	public IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> Tags { get; }

	internal PoserWeights(
		DateTimeOffset creationTimestamp,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		ImageComposition? composition,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
		: base(creationTimestamp, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}