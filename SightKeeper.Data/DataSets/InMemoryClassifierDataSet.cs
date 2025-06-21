using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets;

internal sealed class InMemoryClassifierDataSet(TagFactory<Tag> tagFactory, AssetFactory<ClassifierAsset> assetFactory) : ClassifierDataSet
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public InMemoryTagsLibrary<Tag> TagsLibrary { get; } = new(tagFactory);
	public InMemoryAssetsLibrary<ClassifierAsset> AssetsLibrary { get; } = new(assetFactory);
	public InMemoryWeightsLibrary WeightsLibrary { get; } = new();
	TagsOwner<Tag> DataSet.TagsLibrary => TagsLibrary;
	WeightsLibrary DataSet.WeightsLibrary => WeightsLibrary;
	AssetsOwner<ClassifierAsset> ClassifierDataSet.AssetsLibrary => AssetsLibrary;
}