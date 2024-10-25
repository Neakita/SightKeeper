using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Data.Binary.Services;

public sealed class LockingDataSetEditor : DataSetEditor
{
	public LockingDataSetEditor(AppDataEditingLock locker)
	{
		_locker = locker;
	}

	public override void Edit(DataSet dataSet, DataSetData data, IReadOnlyCollection<TagData> tagsData)
	{
		lock (_locker)
			base.Edit(dataSet, data, tagsData);
	}

	private readonly AppDataEditingLock _locker;
}