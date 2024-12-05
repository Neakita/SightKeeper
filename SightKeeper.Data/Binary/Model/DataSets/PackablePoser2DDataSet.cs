using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Conversion.DataSets.Poser;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DDataSet : PackableDataSet
{
	public required ImmutableArray<PackablePoserTag> Tags { get; init; }
	public required ImmutableArray<PackableItemsAsset<PackablePoser2DItem>> Assets { get; init; }
	public required ImmutableArray<PackablePoserWeights> Weights { get; init; }
}