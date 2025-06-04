using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Application.Tests.Annotation;

internal sealed class FakeAssetsMaker : AssetsMaker
{
	public TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, Image image)
	{
		return assetsOwner.MakeAsset(image);
	}
}