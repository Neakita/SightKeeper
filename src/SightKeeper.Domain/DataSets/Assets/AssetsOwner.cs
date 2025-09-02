using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsOwner<out TAsset> : AssetsContainer<TAsset>
{
	TAsset MakeAsset(ManagedImage image);
	void DeleteAsset(ManagedImage image);
	void ClearAssets();

	TAsset GetOrMakeAsset(ManagedImage image)
	{
		return GetOptionalAsset(image) ?? MakeAsset(image);
	}
}