using MemoryPack;
using SightKeeper.Domain.DataSets.Poser;

namespace SightKeeper.Data.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="DomainPoserTag"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoserTag : PackableTag
{
	public required IReadOnlyCollection<PackableTag> KeyPointTags { get; init; }
}