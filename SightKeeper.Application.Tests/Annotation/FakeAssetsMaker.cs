using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Tests.Annotation;

internal sealed class FakeAssetsMaker : AssetsMaker
{
	public TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, DomainImage image)
	{
		return assetsOwner.MakeAsset(image);
	}
}