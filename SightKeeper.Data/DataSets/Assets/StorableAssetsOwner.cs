using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

public interface StorableAssetsOwner<out TAsset> : StorableAssetsContainer<TAsset>, AssetsOwner<TAsset>
{
	TAsset MakeAsset(StorableImage image);
	void DeleteAsset(StorableImage image);
	void EnsureCapacity(int capacity);

	TAsset GetOrMakeAsset(StorableImage image)
	{
		return GetOptionalAsset(image) ?? MakeAsset(image);
	}

	TAsset AssetsOwner<TAsset>.MakeAsset(Image image)
	{
		return MakeAsset((StorableImage)image);
	}

	void AssetsOwner<TAsset>.DeleteAsset(Image image)
	{
		DeleteAsset((StorableImage)image);
	}

	TAsset AssetsOwner<TAsset>.GetOrMakeAsset(Image image)
	{
		return GetOrMakeAsset((StorableImage)image);
	}
}