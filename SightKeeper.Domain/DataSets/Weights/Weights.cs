using SightKeeper.Domain.DataSets.Weights.ImageCompositions;

namespace SightKeeper.Domain.DataSets.Weights;

public abstract class Weights
{
	public required Model Model { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required ModelSize ModelSize { get; init; }
	public required WeightsMetrics Metrics { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
	public required ImageComposition? Composition { get; init; }
}