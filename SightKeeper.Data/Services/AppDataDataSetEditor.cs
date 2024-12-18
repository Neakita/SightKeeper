using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Services;

public sealed class AppDataDataSetEditor : DataSetEditor
{
	public AppDataDataSetEditor(AppDataAccess appDataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_appDataAccess = appDataAccess;
		_appDataLock = appDataLock;
	}

	public override void Edit(DataSet dataSet, DataSetData data, IReadOnlyCollection<TagData> tagsData)
	{
		lock (_appDataLock)
		{
			base.Edit(dataSet, data, tagsData);
			_appDataAccess.SetDataChanged();
		}
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Lock _appDataLock;
}