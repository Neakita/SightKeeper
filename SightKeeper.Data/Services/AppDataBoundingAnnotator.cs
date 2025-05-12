using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingAnnotator : BoundingAnnotator, ObservableBoundingAnnotator, ObservableAnnotator, IDisposable
{
	public IObservable<(ItemsMaker<AssetItem> asset, AssetItem item)> ItemCreated => _itemCreated.AsObservable();

	public IObservable<Image> AssetsChanged => _assetsChanged;

	public AppDataBoundingAnnotator([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public AssetItem CreateItem(AssetsMaker<ItemsMaker<AssetItem>> assetsLibrary, Image image, Tag tag, Bounding bounding)
	{
		AssetItem item;
		ItemsMaker<AssetItem> asset;
		lock (_appDataLock)
		{
			asset = assetsLibrary.GetOrMakeAsset(image);
			item = asset.MakeItem(tag, bounding);
		}
		_appDataAccess.SetDataChanged();
		_assetsChanged.OnNext(image);
		_itemCreated.OnNext((asset, item));
		return item;
	}

	public void Dispose()
	{
		_itemCreated.Dispose();
		_assetsChanged.Dispose();
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;

	private readonly Subject<(ItemsMaker<AssetItem>, AssetItem)> _itemCreated = new();
	private readonly Subject<Image> _assetsChanged = new();
}