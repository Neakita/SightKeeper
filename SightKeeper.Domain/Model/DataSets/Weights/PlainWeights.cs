using System.Collections.ObjectModel;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class PlainWeights : Weights
{
	public ReadOnlyCollection<Tag> Tags { get; }

	internal PlainWeights(
		DateTimeOffset creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		ReadOnlyCollection<Tag> tags)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}