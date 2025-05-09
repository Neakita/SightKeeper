using System.Collections.ObjectModel;
using MemoryPack;
using SightKeeper.Data.Model.DataSets.Compositions;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="Weights"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableWeights
{
	public required ushort Id { get; init; }
	public required Domain.DataSets.Weights.Model Model { get; init; }
	public required DateTimeOffset CreationTimestamp { get; init; }
	public required ModelSize ModelSize { get; init; }
	public required WeightsMetrics Metrics { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
	public required PackableComposition? Composition { get; init; }
	public required IReadOnlyCollection<byte> TagsIndexes { get; init; }
	public IReadOnlyCollection<KeyPointTagLocation> KeyPointTagsLocations { get; init; } = ReadOnlyCollection<KeyPointTagLocation>.Empty;
}