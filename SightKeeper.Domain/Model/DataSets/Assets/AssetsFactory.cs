using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public interface AssetsFactory<TAsset> where TAsset : Asset
{
	static abstract TAsset Create(Screenshot<TAsset> screenshot);
}