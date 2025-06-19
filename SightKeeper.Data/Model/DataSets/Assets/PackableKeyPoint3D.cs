using MemoryPack;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint3D"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableKeyPoint3D : PackableKeyPoint
{
	public required bool IsVisible { get; init; }
}