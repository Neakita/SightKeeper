using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal partial class SerializablePoser2DTag : SerializableTag
{
	public ImmutableArray<SerializableTag> KeyPoints { get; }
	public ImmutableArray<Poser.SerializableNumericItemProperty> Properties { get; }

	[MemoryPackConstructor]
	public SerializablePoser2DTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<SerializableTag> keyPoints,
		ImmutableArray<Poser.SerializableNumericItemProperty> properties)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
		Properties = properties;
	}

	public SerializablePoser2DTag(Id id, Tag tag, ImmutableArray<SerializableTag> keyPoints, ImmutableArray<Poser.SerializableNumericItemProperty> properties) : base(id, tag)
	{
		KeyPoints = keyPoints;
		Properties = properties;
	}
}