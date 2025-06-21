using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Data.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DomainPoser2DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackablePoserTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableItemsAsset<PackablePoser2DItem>> Assets { get; init; }
}