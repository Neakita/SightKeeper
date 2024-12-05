using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Model.DataSets.Weights;

/// <summary>
/// MemoryPackable version of <see cref="Weights"/>
/// </summary>
[MemoryPackable]
internal partial class PackableWeights
{
	public required ushort Id { get; init; }
	public required DateTimeOffset CreationDate { get; init; }
	public required ModelSize ModelSize { get; init; }
	public required WeightsMetrics Metrics { get; init; }
	public required Vector2<ushort> Resolution { get; init; }
	public required PackableComposition? Composition { get; init; }
}