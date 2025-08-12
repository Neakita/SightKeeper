using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsOwner<out TAsset> : AssetsContainer<TAsset>
{
	TAsset MakeAsset(Image image);
	void DeleteAsset(Image image);
	void ClearAssets();

	TAsset GetOrMakeAsset(Image image)
	{
		return GetOptionalAsset(image) ?? MakeAsset(image);
	}
}