namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class AssetsFactory<TAsset> where TAsset : Asset
{
	public abstract TAsset CreateAsset();
}