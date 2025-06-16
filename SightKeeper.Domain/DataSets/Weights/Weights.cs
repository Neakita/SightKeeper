using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public sealed class Weights
{
	public required Model Model { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required ModelSize ModelSize { get; init; }
	public required WeightsMetrics Metrics { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
	public IReadOnlyCollection<DomainTag> Tags { get; }

	public Weights(params IEnumerable<DomainTag> tags)
	{
		Tags = tags.ToList();
	}
}