using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier;

internal sealed class InMemoryClassifierDataSet(
	TagFactory<Tag> tagFactory,
	AssetFactory<ClassifierAsset> assetFactory,
	WeightsWrapper weightsWrapper)
	: ClassifierDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TagsOwner<Tag> TagsLibrary { get; } = new InMemoryTagsLibrary<Tag>(tagFactory);
	public AssetsOwner<ClassifierAsset> AssetsLibrary { get; } = new InMemoryAssetsLibrary<ClassifierAsset>(assetFactory);
	public WeightsLibrary WeightsLibrary { get; } = new InMemoryWeightsLibrary(weightsWrapper);
}