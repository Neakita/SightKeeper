using System.Collections.ObjectModel;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class PoserWeights : Weights
{
	public ReadOnlyDictionary<PoserTag, ReadOnlyCollection<Tag>> Tags { get; }

	internal PoserWeights(
		DateTimeOffset creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		ReadOnlyDictionary<PoserTag, ReadOnlyCollection<Tag>> tags)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}