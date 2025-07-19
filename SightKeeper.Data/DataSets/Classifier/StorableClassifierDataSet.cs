using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;

namespace SightKeeper.Data.DataSets.Classifier;

public interface StorableClassifierDataSet : ClassifierDataSet
{
	new AssetsOwner<StorableClassifierAsset> AssetsLibrary { get; }

	AssetsOwner<ClassifierAsset> ClassifierDataSet.AssetsLibrary => AssetsLibrary;
}