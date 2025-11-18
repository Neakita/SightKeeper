namespace SightKeeper.Domain.DataSets.Artifacts;

public sealed class ArtifactMetadata
{
	public required string Model { get; init; }
	public required string Format { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
}