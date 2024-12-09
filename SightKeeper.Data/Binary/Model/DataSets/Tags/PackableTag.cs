using MemoryPack;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="Tag"/>
/// </summary>
[MemoryPackable]
internal partial class PackableTag
{
	public required string Name { get; init; }
	public required uint Color { get; init; }
}