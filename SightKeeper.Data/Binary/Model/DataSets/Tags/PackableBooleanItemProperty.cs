using MemoryPack;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="BooleanItemProperty"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableBooleanItemProperty : PackableItemProperty
{
	public PackableBooleanItemProperty(string name) : base(name)
	{
	}
}