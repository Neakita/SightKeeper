using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="PoserTag"/>
/// </summary>
internal sealed class PackablePoserTag : PackableTag
{
	public required IReadOnlyCollection<PackableTag> KeyPointTags { get; init; }
}