using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
internal partial class Tag
{
	public Id Id { get; }
	public string Name { get; }
	public uint Color { get; }

	[MemoryPackConstructor]
	public Tag(Id id, string name, uint color)
	{
		Id = id;
		Name = name;
		Color = color;
	}

	public Tag(Id id, Domain.Model.DataSets.Tags.Tag tag)
	{
		Id = id;
		Name = tag.Name;
		Color = tag.Color;
	}
}