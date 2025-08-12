using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class StorableAssetsOwnerExtension<TTag, TExtendedTag>(
	AssetsOwner<TTag> inner,
	StorableAssetsOwner<TExtendedTag> extendedInner)
	: StorableAssetsOwner<TExtendedTag>
	where TTag : notnull
	where TExtendedTag : TTag
{
	public IReadOnlyCollection<TExtendedTag> Assets => (IReadOnlyCollection<TExtendedTag>)inner.Assets;
	public IReadOnlyCollection<StorableImage> Images => extendedInner.Images;

	public bool Contains(StorableImage image)
	{
		return extendedInner.Contains(image);
	}

	public TExtendedTag GetAsset(StorableImage image)
	{
		return (TExtendedTag)inner.GetAsset(image);
	}

	public TExtendedTag? GetOptionalAsset(StorableImage image)
	{
		return (TExtendedTag?)inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		inner.ClearAssets();
	}

	public TExtendedTag MakeAsset(StorableImage image)
	{
		return (TExtendedTag)inner.MakeAsset(image);
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