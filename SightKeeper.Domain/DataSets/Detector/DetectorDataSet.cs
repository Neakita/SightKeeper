using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.DataSets.Detector;

public interface DetectorDataSet : DataSet
{
	new AssetsOwner<DetectorAsset> AssetsLibrary { get; }
	AssetsOwner<Asset> DataSet.AssetsLibrary => AssetsLibrary;
}