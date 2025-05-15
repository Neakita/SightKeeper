using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingAnnotator : BoundingAnnotator, ObservableBoundingAnnotator, IDisposable
{
	public IObservable<(ItemsMaker<AssetItem> asset, AssetItem item)> ItemCreated => _itemCreated.AsObservable();

	public AppDataBoundingAnnotator([Tag(typeof(AppData))] Lock appDataLock, ChangeListener changeListener, AssetsMaker assetsMaker)
	{
		_appDataLock = appDataLock;
		_changeListener = changeListener;
		_assetsMaker = assetsMaker;
	}

	public AssetItem CreateItem(AssetsOwner<ItemsMaker<AssetItem>> assetsLibrary, Image image, Tag tag, Bounding bounding)
	{
		AssetItem item;
		ItemsMaker<AssetItem> asset;
		lock (_appDataLock)
		{
			asset = _assetsMaker.GetOrMakeAsset(assetsLibrary, image);
			item = asset.MakeItem(tag);
			item.Bounding = bounding;
		}
		_changeListener.SetDataChanged();
		_itemCreated.OnNext((asset, item));
		return item;
	}

	public void Dispose()
	{
		_itemCreated.Dispose();
	}

	private readonly Lock _appDataLock;
	private readonly ChangeListener _changeListener;
	private readonly AssetsMaker _assetsMaker;

	private readonly Subject<(ItemsMaker<AssetItem>, AssetItem)> _itemCreated = new();
}