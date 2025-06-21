using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class LockingAssetsLibrary<TAsset>(AssetsOwner<TAsset> inner, Lock editingLock) : AssetsOwner<TAsset>
{
	public IReadOnlyCollection<TAsset> Assets => inner.Assets;

	public IReadOnlyCollection<Image> Images => inner.Images;

	public TAsset GetAsset(Image image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(Image image)
	{
		return inner.GetOptionalAsset(image);
	}

	public bool Contains(Image image)
	{
		return inner.Contains(image);
	}

	public TAsset MakeAsset(Image image)
	{
		lock (editingLock)
			return inner.MakeAsset(image);
	}

	public void DeleteAsset(Image image)
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