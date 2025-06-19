using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public interface Weights : TagUser
{
	Model Model { get; }
	DateTimeOffset CreationTimestamp { get; }
	ModelSize ModelSize { get; }
	WeightsMetrics Metrics { get; }
	Vector2<ushort> Resolution { get; }
	IReadOnlyList<Tag> Tags { get; }
}