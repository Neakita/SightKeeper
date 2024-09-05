using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="PoserTag"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DTag : PackableTag
{
	public ImmutableArray<PackableTag> KeyPointTags { get; }

	public PackablePoser2DTag(
		byte id,
		string name,
		uint color,
		ImmutableArray<PackableTag> keyPointTags)
		: base(id, name, color)
	{
		KeyPointTags = keyPointTags;
	}
}