namespace SightKeeper.Domain.DataSets.Assets;

public abstract class AssetsFactory<TAsset> where TAsset : Asset
{
	public abstract TAsset CreateAsset();
}