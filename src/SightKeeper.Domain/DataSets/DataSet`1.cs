using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Domain.DataSets;

public interface DataSet<out TAsset> : DataSet where TAsset : Asset
{
	new AssetsOwner<TAsset> AssetsLibrary { get; }

	AssetsOwner<Asset> DataSet.AssetsLibrary => (AssetsOwner<Asset>)AssetsLibrary;
}