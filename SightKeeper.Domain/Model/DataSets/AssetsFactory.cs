namespace SightKeeper.Domain.Model.DataSets;

public interface AssetsFactory<TAsset> where TAsset : Asset
{
	static abstract TAsset Create(Screenshot<TAsset> screenshot);
}