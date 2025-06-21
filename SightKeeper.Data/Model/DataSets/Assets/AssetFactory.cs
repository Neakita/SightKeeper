using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Model.DataSets.Assets;

public interface AssetFactory<out TAsset>
{
	TAsset CreateAsset(Image image);
}