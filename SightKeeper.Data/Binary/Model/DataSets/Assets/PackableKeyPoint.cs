using MemoryPack;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Assets;

/// <summary>
/// MemoryPackable version of <see cref="KeyPoint"/>
/// </summary>
[MemoryPackable]
internal partial class PackableKeyPoint
{
	public required byte TagIndex { get; init; }
	public required Vector2<double> Position { get; init; }
}