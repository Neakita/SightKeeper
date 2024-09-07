using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.DataSets.Assets;
using SightKeeper.Data.Binary.Model.DataSets.Compositions;
using SightKeeper.Data.Binary.Model.DataSets.Tags;
using SightKeeper.Data.Binary.Model.DataSets.Weights;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Data.Binary.Model.DataSets;

/// <summary>
/// MemoryPackable version of <see cref="ClassifierDataSet"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableClassifierDataSet : PackableDataSet<PackableTag, PackableClassifierAsset, PackablePlainWeights>
{
	public PackableClassifierDataSet(
		string name,
		string description,
		ushort? gameId,
		PackableComposition? composition,
		ushort? maxScreenshotsWithoutAsset,
		ImmutableArray<PackableScreenshot> screenshots,
		ImmutableArray<PackableTag> tags,
		ImmutableArray<PackableClassifierAsset> assets,
		ImmutableArray<PackablePlainWeights> weights)
		: base(name, description, gameId, composition, maxScreenshotsWithoutAsset, screenshots, tags, assets, weights)
	{
	}
}