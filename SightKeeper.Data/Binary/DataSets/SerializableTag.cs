using FlakeId;
using MemoryPack;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
public sealed partial class SerializableTag
{
	public Id Id { get; }
	public string Name { get; }
	public uint Color { get; }

	[MemoryPackConstructor]
	public SerializableTag(Id id, string name, uint color)
	{
		Id = id;
		Name = name;
		Color = color;
	}

	public SerializableTag(Id id, Tag tag)
	{
		Id = id;
		Name = tag.Name;
		Color = tag.Color;
	}
}