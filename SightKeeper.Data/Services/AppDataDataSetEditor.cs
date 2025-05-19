using SightKeeper.Application;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Data.Services;

public sealed class AppDataDataSetEditor : DataSetEditor
{
	public required ChangeListener ChangeListener { get; init; }
	[Tag(typeof(AppData))] public required Lock AppDataLock { get; init; }

	public override void Edit(ExistingDataSetData data)
	{
		lock (AppDataLock)
			base.Edit(data);
		ChangeListener.SetDataChanged();
	}
}