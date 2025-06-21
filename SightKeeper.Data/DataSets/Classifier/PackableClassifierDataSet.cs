using MemoryPack;
using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackableTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableClassifierAsset> Assets { get; init; }
}