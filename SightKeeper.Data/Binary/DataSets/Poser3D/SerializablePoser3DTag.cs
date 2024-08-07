using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.DataSets.Poser3D;

[MemoryPackable]
internal sealed partial class SerializablePoser3DTag : SerializableTag
{
	public ImmutableArray<SerializableTag> KeyPoints { get; }
	public ImmutableArray<SerializableNumericItemProperty> NumericProperties { get; }
	public ImmutableArray<string> BooleanProperties { get; }

	[MemoryPackConstructor]
	public SerializablePoser3DTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<SerializableTag> keyPoints,
		ImmutableArray<SerializableNumericItemProperty> numericProperties,
		ImmutableArray<string> booleanProperties)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}

	public SerializablePoser3DTag(
		Id id,
		Tag tag,
		ImmutableArray<SerializableTag> keyPoints,
		ImmutableArray<SerializableNumericItemProperty> numericProperties,
		ImmutableArray<string> booleanProperties) : base(id, tag)
	{
		KeyPoints = keyPoints;
		NumericProperties = numericProperties;
		BooleanProperties = booleanProperties;
	}
}