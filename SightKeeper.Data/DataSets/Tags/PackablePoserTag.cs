using MemoryPack;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="PoserTag"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoserTag : PackableTag
{
	public required IReadOnlyCollection<PackableTag> KeyPointTags { get; init; }
}