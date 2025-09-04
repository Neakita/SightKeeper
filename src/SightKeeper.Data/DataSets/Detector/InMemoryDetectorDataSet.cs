using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Detector;

internal sealed class InMemoryDetectorDataSet(
	TagFactory<Tag> tagFactory,
	AssetFactory<ItemsAsset<DetectorItem>> assetFactory,
	WeightsWrapper weightsWrapper)
	: DetectorDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TagsOwner<Tag> TagsLibrary { get; } =
		new InMemoryTagsLibrary<Tag>(tagFactory);
	public AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; } =
		new InMemoryAssetsLibrary<ItemsAsset<DetectorItem>>(assetFactory);
	public WeightsLibrary WeightsLibrary { get; } =
		new InMemoryWeightsLibrary(weightsWrapper);
}