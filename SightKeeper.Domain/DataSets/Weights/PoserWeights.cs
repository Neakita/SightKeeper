using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class PoserWeights : Weights
{
	public IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> Tags { get; }

	internal PoserWeights(
		DateTimeOffset creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}