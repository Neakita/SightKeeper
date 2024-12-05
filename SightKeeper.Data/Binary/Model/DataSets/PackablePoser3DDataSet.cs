using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DDataSet : PackableDataSet
{
	public required ImmutableArray<PackablePoserTag> Tags { get; init; }
	public required ImmutableArray<PackableItemsAsset<PackablePoser3DItem>> Assets { get; init; }
	public required ImmutableArray<PackablePoserWeights> Weights { get; init; }
}