using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingAnnotator : BoundingAnnotator
{
	public AppDataBoundingAnnotator([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public BoundedItem CreateItem(AssetsMaker<ItemsCreator> assetsLibrary, Screenshot screenshot, Tag tag, Bounding bounding)
	{
		BoundedItem item;
		lock (_appDataLock)
		{
			var asset = assetsLibrary.GetOrMakeAsset(screenshot);
			item = asset.CreateItem(tag, bounding);
		}
		_appDataAccess.SetDataChanged();
		return item;
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;
}