using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="PoserTag"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DTag : PackablePoserTag
{
	public ImmutableArray<PackableNumericItemProperty> NumericProperties { get; }
	
	public PackablePoser2DTag(
		byte id,
		string name,
		uint color,
		ImmutableArray<PackableTag> keyPointTags,
		ImmutableArray<PackableNumericItemProperty> numericProperties)
		: base(id, name, color, keyPointTags)
	{
		NumericProperties = numericProperties;
	}
}