using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

public interface AssetFactory<out TAsset>
{
	TAsset CreateAsset(Image image);
}