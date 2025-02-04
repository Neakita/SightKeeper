using MemoryPack;
using SightKeeper.Data.Model.DataSets.Assets;
using SightKeeper.Data.Model.DataSets.Tags;
using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Poser3D;

namespace SightKeeper.Data.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DDataSet : PackableDataSet
{
	public required IReadOnlyCollection<PackablePoserTag> Tags { get; init; }
	public required IReadOnlyCollection<PackableItemsAsset<PackablePoser3DItem>> Assets { get; init; }
	public required IReadOnlyCollection<PackablePoserWeights> Weights { get; init; }
}