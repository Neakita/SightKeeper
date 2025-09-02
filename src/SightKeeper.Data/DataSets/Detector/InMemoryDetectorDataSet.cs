using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Assets.Items;
using SightKeeper.Data.DataSets.Detector.Items;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector;

internal sealed class InMemoryDetectorDataSet(
	TagFactory<StorableTag> tagFactory,
	AssetFactory<StorableItemsAsset<StorableDetectorItem>> assetFactory,
	WeightsWrapper weightsWrapper)
	: StorableDetectorDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public StorableTagsOwner<StorableTag> TagsLibrary { get; } =
		new InMemoryTagsLibrary<StorableTag>(tagFactory);
	public StorableAssetsOwner<StorableItemsAsset<StorableDetectorItem>> AssetsLibrary { get; } =
		new InMemoryAssetsLibrary<StorableItemsAsset<StorableDetectorItem>>(assetFactory);
	public StorableWeightsLibrary WeightsLibrary { get; } =
		new InMemoryWeightsLibrary(weightsWrapper);
	public StorableDetectorDataSet Innermost => this;
}