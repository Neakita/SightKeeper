using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="Tag"/>
/// </summary>
[MemoryPackable]
internal partial class PackableTag
{
	public string Name { get; }
	public uint Color { get; }

	public PackableTag(string name, uint color)
	{
		Name = name;
		Color = color;
	}
}