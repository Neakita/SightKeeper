using SightKeeper.Data.ImageSets.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class TrackableAssetsLibrary<TAsset>(StorableAssetsOwner<TAsset> inner, ChangeListener listener) : StorableAssetsOwner<TAsset>
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
		var asset = inner.MakeAsset(image);
		listener.SetDataChanged();
		return asset;
	}

	public void DeleteAsset(StorableImage image)
	{
		inner.DeleteAsset(image);
		listener.SetDataChanged();
	}

	public void ClearAssets()
	{
		inner.ClearAssets();
		listener.SetDataChanged();
	}
}