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
	public ImmutableArray<PackableNumericItemProperty> NumericProperties { get; }
	public ImmutableArray<PackableBooleanItemProperty> BooleanProperties { get; }
	
	public PackablePoser3DTag(
		byte id,
		string name,
		uint color,
		ImmutableArray<PackableTag> keyPointTags,
		ImmutableArray<PackableNumericItemProperty> numericProperties,
		ImmutableArray<PackableBooleanItemProperty> booleanProperties)
		: base(id, name, color, keyPointTags)
	{
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}
}