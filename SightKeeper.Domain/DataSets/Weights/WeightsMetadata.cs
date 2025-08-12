namespace SightKeeper.Domain.DataSets.Weights;

public sealed class WeightsMetadata
{
	public Model Model { get; init; }
	public DateTimeOffset CreationTimestamp { get; init; }
	public ModelSize ModelSize { get; init; }
	public WeightsMetrics Metrics { get; init; }
	public Vector2<ushort> Resolution { get; init; }
}