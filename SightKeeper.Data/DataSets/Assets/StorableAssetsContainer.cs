using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

public interface StorableAssetsContainer<out TAsset> : AssetsContainer<TAsset>
{
	new IReadOnlyCollection<StorableImage> Images { get; }
	TAsset GetAsset(StorableImage image);
	TAsset? GetOptionalAsset(StorableImage image);
	bool Contains(StorableImage image);

	IReadOnlyCollection<Image> AssetsContainer<TAsset>.Images => Images;

	TAsset AssetsContainer<TAsset>.GetAsset(Image image)
	{
		return GetAsset((StorableImage)image);
	}

	TAsset? AssetsContainer<TAsset>.GetOptionalAsset(Image image)
	{
		return GetOptionalAsset((StorableImage)image);
	}

	bool AssetsContainer<TAsset>.Contains(Image image)
	{
		return Contains((StorableImage)image);
	}
}