using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DItem
{
	public required byte TagId { get; init; }
	public required Bounding Bounding { get; init; }
	public required ImmutableArray<PackableKeyPoint> KeyPoints { get; init; }
}