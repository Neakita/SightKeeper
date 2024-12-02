using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="PoserTag"/>
/// </summary>
internal abstract class PackablePoserTag : PackableTag
{
	public ImmutableArray<PackableTag> KeyPointTags { get; }

	protected PackablePoserTag(byte id, string name, uint color, ImmutableArray<PackableTag> keyPointTags) : base(id, name, color)
	{
		KeyPointTags = keyPointTags;
	}
}