using System.Collections.Immutable;
using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets.Poser;

[MemoryPackable]
internal partial class SerializablePoserTag : SerializableTag
{
	public ImmutableArray<SerializableTag> KeyPoints { get; }

	[MemoryPackConstructor]
	public SerializablePoserTag(
		Id id,
		string name,
		uint color,
		ImmutableArray<SerializableTag> keyPoints)
		: base(id, name, color)
	{
		KeyPoints = keyPoints;
	}

	public SerializablePoserTag(Id id, Tag tag, ImmutableArray<SerializableTag> keyPoints) : base(id, tag)
	{
		KeyPoints = keyPoints;
	}
}