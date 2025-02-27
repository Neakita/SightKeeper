using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingEditor : BoundingEditor
{
	public AppDataBoundingEditor([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public void SetBounding(BoundedItem item, Bounding bounding)
	{
		lock (_appDataLock)
			item.Bounding = bounding;
		_appDataAccess.SetDataChanged();
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;
}