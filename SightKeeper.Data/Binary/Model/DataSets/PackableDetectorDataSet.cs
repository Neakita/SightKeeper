using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="DetectorDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableDetectorDataSet : PackableDataSet<PackableTag, PackableItemsAsset<PackableDetectorItem>, PackablePlainWeights>
{
	public PackableDetectorDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableItemsAsset<PackableDetectorItem>> assets,
		ImmutableArray<PackablePlainWeights> weights)
		: base(name, description, gameId, composition, screenshots, tags, assets, weights)
	{
	}
}