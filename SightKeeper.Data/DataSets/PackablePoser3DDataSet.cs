using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DomainPoser3DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackablePoserTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableItemsAsset<PackablePoser3DItem>> Assets { get; init; }
}