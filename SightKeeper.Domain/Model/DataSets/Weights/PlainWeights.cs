using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public sealed class PlainWeights : Weights
{
	public IReadOnlyCollection<Tag> Tags { get; }

	internal PlainWeights(
		DateTimeOffset creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition,
		IReadOnlyCollection<Tag> tags)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags;
	}
}