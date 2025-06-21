using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Detector;

namespace SightKeeper.Data.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DomainDetectorDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDetectorDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackableTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableItemsAsset<PackableDetectorItem>> Assets { get; init; }
}