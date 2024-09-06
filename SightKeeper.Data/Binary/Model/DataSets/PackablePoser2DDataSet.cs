using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser2DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser2DDataSet : PackableDataSet<PackablePoser2DTag, PackableItemsAsset<PackablePoser2DItem>, PackablePoserWeights>
{
	public PackablePoser2DDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		IEnumerable<PackableTag> tags,
		IEnumerable<PackableAsset> assets,
		IEnumerable<PackableWeights> weights)
		: base(name, description, gameId, composition, screenshots, tags, assets, weights)
	{
	}
}