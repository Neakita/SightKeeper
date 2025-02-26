using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsMaker<out TAsset>
{
	TAsset MakeAsset(Image image);
	TAsset GetOrMakeAsset(Image image);
}