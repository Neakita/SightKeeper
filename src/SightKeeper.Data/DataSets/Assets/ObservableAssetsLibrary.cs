using SightKeeper.Data.ImageSets.Images;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class ObservableAssetsLibrary<TAsset>(StorableAssetsOwner<TAsset> inner) : StorableAssetsOwner<TAsset>
{
	public IReadOnlyCollection<TAsset> Assets => _assets;
	public IReadOnlyCollection<StorableImage> Images => _images;

	public bool Contains(StorableImage image)
	{
		return inner.Contains(image);
	}

	public TAsset GetAsset(StorableImage image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(StorableImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		inner.ClearAssets();
	}

	public TAsset MakeAsset(StorableImage image)
	{
		var asset = inner.MakeAsset(image);
		var assetsChange = new Addition<TAsset>
		{
			Items = [asset]
		};
		var imagesChange = new Addition<StorableImage>
		{
			Items = [image]
		};
		_assets.Notify(assetsChange);
		_images.Notify(imagesChange);
		return asset;
	}

	public void DeleteAsset(StorableImage image)
	{
		var asset = GetAsset(image);
		inner.DeleteAsset(image);
		var assetsChange = new Addition<TAsset>
		{
			Items = [asset]
		};
		var imagesChange = new Addition<StorableImage>
		{
			Items = [image]
		};
		_assets.Notify(assetsChange);
		_images.Notify(imagesChange);
	}

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}

	private readonly ExternalObservableCollection<TAsset> _assets = new(inner.Assets);
	private readonly ExternalObservableCollection<StorableImage> _images = new(inner.Images);
}