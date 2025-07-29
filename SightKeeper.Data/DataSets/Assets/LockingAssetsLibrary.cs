using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class LockingAssetsLibrary<TAsset>(StorableAssetsOwner<TAsset> inner, Lock editingLock) : StorableAssetsOwner<TAsset>
{
	public IReadOnlyCollection<TAsset> Assets => inner.Assets;

	public IReadOnlyCollection<StorableImage> Images => inner.Images;

	public TAsset GetAsset(StorableImage image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(StorableImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public bool Contains(StorableImage image)
	{
		return inner.Contains(image);
	}

	public TAsset MakeAsset(StorableImage image)
	{
		lock (editingLock)
			return inner.MakeAsset(image);
	}

	public void DeleteAsset(StorableImage image)
	{
		lock (editingLock)
			inner.DeleteAsset(image);
	}

	public void ClearAssets()
	{
		lock (editingLock)
			inner.ClearAssets();
	}
}