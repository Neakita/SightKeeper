using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.DataSets.Classifier;

public interface ClassifierDataSet : DataSet
{
	new AssetsOwner<DomainClassifierAsset> AssetsLibrary { get; }
	AssetsOwner<Asset> DataSet.AssetsLibrary => AssetsLibrary;
}