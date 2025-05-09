using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsOwner<out TAsset> : AssetsMaker<TAsset>, AssetsContainer<TAsset>
{
	void DeleteAsset(Image image);
}