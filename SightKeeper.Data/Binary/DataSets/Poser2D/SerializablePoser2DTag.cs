using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser2D;

[MemoryPackable]
internal partial class SerializablePoser2DTag : SerializableTag
{
	public ImmutableArray<SerializableTag> KeyPoints { get; }

	[MemoryPackConstructor]
	public SerializablePoser2DTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<SerializableTag> keyPoints)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
	}

	public SerializablePoser2DTag(Id id, Tag tag, ImmutableArray<SerializableTag> keyPoints) : base(id, tag)
	{
		KeyPoints = keyPoints;
	}
}