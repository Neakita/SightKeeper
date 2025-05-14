using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Annotation;

public interface AssetsMaker
{
	TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, Image image);

	TAsset GetOrMakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, Image image)
	{
		var asset = assetsOwner.GetOptionalAsset(image);
		if (asset != null)
			return asset;
		return MakeAsset(assetsOwner, image);
	}
}