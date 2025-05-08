using SightKeeper.Domain.Images;

namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsFactory<out TAsset> where TAsset : Asset
{
	TAsset CreateAsset(Image image);
}