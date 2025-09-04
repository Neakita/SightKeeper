using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class LockingAssetsLibrary<TAsset>(AssetsOwner<TAsset> inner, Lock editingLock) : AssetsOwner<TAsset>
{
	public IReadOnlyCollection<TAsset> Assets => inner.Assets;

	public IReadOnlyCollection<ManagedImage> Images => inner.Images;

	public TAsset GetAsset(ManagedImage image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(ManagedImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public bool Contains(ManagedImage image)
	{
		return inner.Contains(image);
	}

	public TAsset MakeAsset(ManagedImage image)
	{
		lock (editingLock)
			return inner.MakeAsset(image);
	}

	public void DeleteAsset(ManagedImage image)
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