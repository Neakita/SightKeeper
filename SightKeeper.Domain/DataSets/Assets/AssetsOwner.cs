using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsOwner<out TAsset> : AssetsContainer<TAsset>
{
	TAsset MakeAsset(Image image);
	TAsset GetOrMakeAsset(Image image);
	void DeleteAsset(Image image);
}