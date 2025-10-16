using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Assets;

internal sealed class ObservableAssetsLibrary<TAsset>(AssetsOwner<TAsset> inner) : AssetsOwner<TAsset>, IDisposable
{
	public IReadOnlyCollection<TAsset> Assets => _assets;
	public IReadOnlyCollection<ManagedImage> Images => _images;

	public bool Contains(ManagedImage image)
	{
		return inner.Contains(image);
	}

	public TAsset GetAsset(ManagedImage image)
	{
		return inner.GetAsset(image);
	}

	public TAsset? GetOptionalAsset(ManagedImage image)
	{
		return inner.GetOptionalAsset(image);
	}

	public void ClearAssets()
	{
		inner.ClearAssets();
	}

	public TAsset MakeAsset(ManagedImage image)
	{
		var asset = inner.MakeAsset(image);
		var assetsChange = new Addition<TAsset>
		{
			Items = [asset]
		};
		var imagesChange = new Addition<ManagedImage>
		{
			Items = [image]
		};
		_assets.Notify(assetsChange);
		_images.Notify(imagesChange);
		return asset;
	}

	public void DeleteAsset(ManagedImage image)
	{
		var asset = GetAsset(image);
		inner.DeleteAsset(image);
		var assetsChange = new Addition<TAsset>
		{
			Items = [asset]
		};
		var imagesChange = new Addition<ManagedImage>
		{
			Items = [image]
		};
		_assets.Notify(assetsChange);
		_images.Notify(imagesChange);
	}

	public void Dispose()
	{
		_assets.Dispose();
		_images.Dispose();
	}

	private readonly ExternalObservableCollection<TAsset> _assets = new(inner.Assets);
	private readonly ExternalObservableCollection<ManagedImage> _images = new(inner.Images);
}