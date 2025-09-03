using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class StorableAssetsOwnerExtension<TAsset, TExtendedAsset>(
	AssetsOwner<TAsset> inner,
	StorableAssetsOwner<TExtendedAsset> extendedInner)
	: StorableAssetsOwner<TExtendedAsset>
	where TAsset : notnull
	where TExtendedAsset : TAsset
{
	public IReadOnlyCollection<TExtendedAsset> Assets => (IReadOnlyCollection<TExtendedAsset>)inner.Assets;
	public IReadOnlyCollection<StorableImage> Images => extendedInner.Images;

	public bool Contains(StorableImage image)
	{
		return extendedInner.Contains(image);
	}

	public TExtendedAsset GetAsset(StorableImage image)
	{
		return (TExtendedAsset)inner.GetAsset(image);
	}

	public TExtendedAsset? GetOptionalAsset(StorableImage image)
	{
		return (TExtendedAsset?)inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		inner.ClearAssets();
	}

	public TExtendedAsset MakeAsset(StorableImage image)
	{
		return (TExtendedAsset)inner.MakeAsset(image);
	}

	public void DeleteAsset(StorableImage image)
	{
		inner.DeleteAsset(image);
	}

	public void EnsureCapacity(int capacity)
	{
		extendedInner.EnsureCapacity(capacity);
	}
}