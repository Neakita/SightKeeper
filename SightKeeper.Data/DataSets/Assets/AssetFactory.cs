using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal interface AssetFactory<out TAsset>
{
	TAsset CreateAsset(StorableImage image);
}