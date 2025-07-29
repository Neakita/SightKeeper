using SightKeeper.Data.DataSets.Assets;
using SightKeeper.Data.DataSets.Weights;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Classifier;

public interface StorableClassifierDataSet : ClassifierDataSet
{
	new StorableAssetsOwner<StorableClassifierAsset> AssetsLibrary { get; }
	new StorableWeightsLibrary WeightsLibrary { get; }

	AssetsOwner<ClassifierAsset> ClassifierDataSet.AssetsLibrary => AssetsLibrary;
	WeightsLibrary DataSet.WeightsLibrary => WeightsLibrary;
}