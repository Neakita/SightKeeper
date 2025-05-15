using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.Services;

public sealed class AppDataBoundingEditor : BoundingEditor
{
	public AppDataBoundingEditor([Tag(typeof(AppData))] Lock appDataLock, ChangeListener changeListener)
	{
		_appDataLock = appDataLock;
		_changeListener = changeListener;
	}

	public void SetBounding(BoundedItem item, Bounding bounding)
	{
		lock (_appDataLock)
			item.Bounding = bounding;
		_changeListener.SetDataChanged();
	}

	private readonly Lock _appDataLock;
	private readonly ChangeListener _changeListener;
}