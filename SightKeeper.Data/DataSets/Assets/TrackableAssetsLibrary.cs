using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class TrackableAssetsLibrary<TAsset>(AssetsOwner<TAsset> inner, ChangeListener listener) : AssetsOwner<TAsset>
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
		var asset = inner.MakeAsset(image);
		listener.SetDataChanged();
		return asset;
	}

	public void DeleteAsset(Image image)
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