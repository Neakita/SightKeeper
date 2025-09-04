using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal interface AssetFactory<out TAsset>
{
	TAsset CreateAsset(ManagedImage image);
}