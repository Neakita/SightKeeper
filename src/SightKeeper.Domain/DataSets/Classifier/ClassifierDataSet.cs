using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.DataSets.Classifier;

public interface ClassifierDataSet : DataSet
{
	new AssetsOwner<ClassifierAsset> AssetsLibrary { get; }
	AssetsOwner<Asset> DataSet.AssetsLibrary => AssetsLibrary;
}