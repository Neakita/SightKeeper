namespace SightKeeper.Domain.Model.DataSets.Assets;

public interface AssetsDestroyer<TAsset>
{
	static abstract void Destroy(TAsset asset);
}