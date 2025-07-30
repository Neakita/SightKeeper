using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class InMemoryClassifierDataSet(
	TagFactory<StorableTag> tagFactory,
	AssetFactory<StorableClassifierAsset> assetFactory,
	WeightsWrapper weightsWrapper)
	: StorableClassifierDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public InMemoryTagsLibrary<StorableTag> TagsLibrary { get; } = new(tagFactory);
	public InMemoryAssetsLibrary<StorableClassifierAsset> AssetsLibrary { get; } = new(assetFactory);
	public InMemoryWeightsLibrary WeightsLibrary { get; } = new(weightsWrapper);

	TagsOwner<StorableTag> StorableClassifierDataSet.TagsLibrary => TagsLibrary;
	StorableAssetsOwner<StorableClassifierAsset> StorableClassifierDataSet.AssetsLibrary => AssetsLibrary;
	StorableWeightsLibrary StorableClassifierDataSet.WeightsLibrary => WeightsLibrary;
}