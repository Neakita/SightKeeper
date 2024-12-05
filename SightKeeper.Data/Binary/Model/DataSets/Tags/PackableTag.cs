using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="Tag"/>
/// </summary>
[MemoryPackable]
internal partial class PackableTag
{
	public required byte Id { get; init; }
	public required string Name { get; init; }
	public required uint Color { get; init; }
}