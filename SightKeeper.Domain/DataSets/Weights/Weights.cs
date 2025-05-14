using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class Weights
{
	public required Model Model { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required ModelSize ModelSize { get; init; }
	public required WeightsMetrics Metrics { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
	public ImageComposition? Composition { get; init; }
	public IReadOnlyCollection<Tag> Tags { get; }

	public Weights(params IEnumerable<Tag> tags)
	{
		Tags = tags.ToList();
	}
}