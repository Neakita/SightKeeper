using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DetectorDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDetectorDataSet : PackableDataSet
{
	public required ImmutableArray<PackableTag> Tags { get; init; }
	public required ImmutableArray<PackableItemsAsset<PackableDetectorItem>> Assets { get; init; }
	public required ImmutableArray<PackablePlainWeights> Weights { get; init; }
}