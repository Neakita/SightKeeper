using MemoryPack;
using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DataSet"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableClassifierDataSet))]
[MemoryPackUnion(1, typeof(PackableDetectorDataSet))]
[MemoryPackUnion(2, typeof(PackablePoser2DDataSet))]
[MemoryPackUnion(3, typeof(PackablePoser3DDataSet))]
internal abstract partial class PackableDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public required IReadOnlyCollection<PackableWeights> Weights { get; init; }
}