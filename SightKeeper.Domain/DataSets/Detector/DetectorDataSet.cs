using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Domain.DataSets.Detector;

public interface DetectorDataSet : DataSet
{
	new AssetsOwner<ItemsAsset<DetectorItem>> AssetsLibrary { get; }
	AssetsOwner<Asset> DataSet.AssetsLibrary => AssetsLibrary;
}