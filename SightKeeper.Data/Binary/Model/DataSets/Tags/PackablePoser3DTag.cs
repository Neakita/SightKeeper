using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DTag"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DTag : PackablePoserTag
{
	public PackablePoser3DTag(
		byte id,
		string name,
		uint color,
		ImmutableArray<PackableTag> keyPointTags)
		: base(id, name, color, keyPointTags)
	{
	}
}