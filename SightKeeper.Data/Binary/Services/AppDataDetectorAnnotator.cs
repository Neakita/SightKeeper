using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataDetectorAnnotator : DetectorAnnotator
{
	public AppDataDetectorAnnotator(AppDataAccess appDataAccess, AppDataEditingLock locker)
	{
		_appDataAccess = appDataAccess;
		_locker = locker;
	}

	public override DetectorItem CreateItem(Screenshot<DetectorAsset> screenshot, DetectorTag tag, Bounding bounding)
	{
		DetectorItem item;
		lock (_locker)
			item = base.CreateItem(screenshot, tag, bounding);
		_appDataAccess.SetDataChanged();
		return item;
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _locker;
}