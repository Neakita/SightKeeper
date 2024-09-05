using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Model.DataSets.Tags;

/// <summary>
/// MemoryPackable version of <see cref="ItemProperty"/>
/// </summary>
internal abstract class PackableItemProperty
{
	public string Name { get; }

	public PackableItemProperty(string name)
	{
		Name = name;
	}
}