using MemoryPack;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Model.DataSets.Tags;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DomainClassifierDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackableTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableClassifierAsset> Assets { get; init; }
}