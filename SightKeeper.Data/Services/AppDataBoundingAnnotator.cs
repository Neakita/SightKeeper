using System.Reactive.Linq;
using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingAnnotator : BoundingAnnotator, ObservableBoundingAnnotator
{
	public IObservable<(ItemsCreator asset, BoundedItem item)> ItemCreated => _itemCreated.AsObservable();

	public AppDataBoundingAnnotator([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public BoundedItem CreateItem(AssetsMaker<ItemsCreator> assetsLibrary, Image image, Tag tag, Bounding bounding)
	{
		BoundedItem item;
		ItemsCreator asset;
		lock (_appDataLock)
		{
			asset = assetsLibrary.GetOrMakeAsset(image);
			item = asset.CreateItem(tag, bounding);
		}
		_appDataAccess.SetDataChanged();
		_itemCreated.OnNext((asset, item));
		return item;
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;

	private readonly Subject<(ItemsCreator, BoundedItem)> _itemCreated = new();
}