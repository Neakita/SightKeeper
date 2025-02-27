namespace SightKeeper.Domain.DataSets.Assets;

public interface AssetsFactory<out TAsset> where TAsset : Asset
{
	TAsset CreateAsset();
}