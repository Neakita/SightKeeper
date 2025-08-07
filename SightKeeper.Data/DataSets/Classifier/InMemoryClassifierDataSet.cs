using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class InMemoryClassifierDataSet(
	TagFactory<StorableTag> tagFactory,
	AssetFactory<StorableClassifierAsset> assetFactory,
	WeightsWrapper weightsWrapper)
	: StorableClassifierDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public StorableTagsOwner<StorableTag> TagsLibrary { get; } = new InMemoryTagsLibrary<StorableTag>(tagFactory);
	public StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; } = new InMemoryAssetsLibrary<StorableClassifierAsset>(assetFactory);
	public StorableWeightsLibrary WeightsLibrary { get; } = new InMemoryWeightsLibrary(weightsWrapper);
	public StorableClassifierDataSet Innermost => this;
}