using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Services;

public sealed class AppDataDataSetEditor : DataSetEditor
{
	public AppDataDataSetEditor(ChangeListener changeListener, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_changeListener = changeListener;
		_appDataLock = appDataLock;
	}

	public override void Edit(DataSet dataSet, DataSetData data)
	{
		lock (_appDataLock)
		{
			base.Edit(dataSet, data);
			_changeListener.SetDataChanged();
		}
	}

	private readonly ChangeListener _changeListener;
	private readonly Lock _appDataLock;
}