using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DItem
{
	public required byte TagIndex { get; init; }
	public required Bounding Bounding { get; init; }
	public required ImmutableArray<PackableKeyPoint3D> KeyPoints { get; init; }
}