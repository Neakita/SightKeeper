namespace SightKeeper.Domain.DataSets.Weights;

public sealed class WeightsMetadata
{
	public string Model { get; init; } = string.Empty;
	public DateTimeOffset CreationTimestamp { get; init; }
	public WeightsMetrics Metrics { get; init; }
	public Vector2<ushort> Resolution { get; init; }
}