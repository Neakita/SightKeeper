namespace SightKeeper.Domain.Model.DataSets;

public interface AssetsDestroyer<TAsset>
{
	static abstract void Destroy(TAsset asset);
}