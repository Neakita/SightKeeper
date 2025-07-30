using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Classifier.Assets;
using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier;

public interface StorableClassifierDataSet : ClassifierDataSet
{
	new TagsOwner<StorableTag> TagsLibrary { get; }
	new StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; }
	new StorableWeightsLibrary WeightsLibrary { get; }

	TagsOwner<Tag> DataSet.TagsLibrary => TagsLibrary;
	AssetsOwner<ClassifierAsset> ClassifierDataSet.AssetsLibrary => AssetsLibrary;
	WeightsLibrary DataSet.WeightsLibrary => WeightsLibrary;
}