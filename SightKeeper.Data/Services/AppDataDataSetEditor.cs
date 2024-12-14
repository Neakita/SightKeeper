using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Services;

public sealed class AppDataDataSetEditor : DataSetEditor
{
	public AppDataDataSetEditor(AppDataAccess appDataAccess, AppDataEditingLock locker)
	{
		_appDataAccess = appDataAccess;
		_locker = locker;
	}

	public override void Edit(DataSet dataSet, DataSetData data, IReadOnlyCollection<TagData> tagsData)
	{
		lock (_locker)
		{
			base.Edit(dataSet, data, tagsData);
			_appDataAccess.SetDataChanged();
		}
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _locker;
}