using MemoryPack;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Model.DataSets.Tags;
using SightKeeper.Domain.DataSets.Poser2D;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackablePoserTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableItemsAsset<PackablePoser2DItem>> Assets { get; init; }
}