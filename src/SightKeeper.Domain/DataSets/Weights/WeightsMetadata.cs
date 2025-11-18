namespace SightKeeper.Domain.DataSets.Weights;

public sealed class WeightsMetadata
{
	public required string Model { get; init; }
	public required string Format { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
}