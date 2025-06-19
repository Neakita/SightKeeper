using MemoryPack;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Model.DataSets.Tags;

namespace SightKeeper.Data.Model.DataSets;

[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackableTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableClassifierAsset> Assets { get; init; }
}