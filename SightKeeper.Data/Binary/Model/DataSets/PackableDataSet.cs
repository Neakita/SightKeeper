using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Composition;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDataSet
{
	public string Name { get; }
	public string Description { get; }
	public ushort? GameId { get; }
	public PackableComposition? Composition { get; }

	public PackableDataSet(string name, string description, ushort? gameId, PackableComposition? composition)
	{
		Name = name;
		Description = description;
		GameId = gameId;
		Composition = composition;
	}
}