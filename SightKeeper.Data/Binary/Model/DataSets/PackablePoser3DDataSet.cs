using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="Poser3DDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackablePoser3DDataSet : PackableDataSet<PackablePoser3DTag, PackableItemsAsset<PackablePoser3DItem>, PackablePoserWeights>
{
	public PackablePoser3DDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ushort? maxScreenshotsWithoutAsset,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackablePoser3DTag> tags,
		ImmutableArray<PackableItemsAsset<PackablePoser3DItem>> assets,
		ImmutableArray<PackablePoserWeights> weights)
		: base(name, description, gameId, composition, maxScreenshotsWithoutAsset, screenshots, tags, assets, weights)
	{
	}
}