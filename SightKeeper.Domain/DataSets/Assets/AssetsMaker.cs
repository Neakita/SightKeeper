using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsMaker<out TAsset>
{
	TAsset MakeAsset(Screenshot screenshot);
	TAsset GetOrMakeAsset(Screenshot screenshot);
}