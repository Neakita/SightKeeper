using FlakeId;
using MemoryPack;

namespace SightKeeper.Data.Binary.DataSets;

[MemoryPackable]
public partial class SerializableTag
{
	public Id Id { get; }
	public string Name { get; }
	public uint Color { get; }

	public SerializableTag(Id id, string name, uint color)
	{
		Id = id;
		Name = name;
		Color = color;
	}
}