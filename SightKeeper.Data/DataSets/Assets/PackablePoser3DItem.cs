using MemoryPack;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DItem
{
	public required byte TagIndex { get; init; }
	public required Bounding Bounding { get; init; }
	public required IReadOnlyCollection<PackableKeyPoint3D> KeyPoints { get; init; }
}