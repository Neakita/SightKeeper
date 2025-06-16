using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Annotation;

public interface AssetsMaker
{
	TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, DomainImage image);

	TAsset GetOrMakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, DomainImage image)
	{
		var asset = assetsOwner.GetOptionalAsset(image);
		if (asset != null)
			return asset;
		return MakeAsset(assetsOwner, image);
	}
}