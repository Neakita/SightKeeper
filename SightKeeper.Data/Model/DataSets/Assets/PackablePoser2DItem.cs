using MemoryPack;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Data.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DItem"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DItem
{
	public required byte TagIndex { get; init; }
	public required Bounding Bounding { get; init; }
	public required IReadOnlyCollection<PackableKeyPoint> KeyPoints { get; init; }
}