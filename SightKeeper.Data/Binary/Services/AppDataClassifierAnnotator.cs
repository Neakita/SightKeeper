using Serilog;
using SightKeeper.Application;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataClassifierAnnotator : ClassifierAnnotator
{
	public AppDataClassifierAnnotator(AppDataAccess appDataAccess, AppDataEditingLock locker, ILogger logger) : base(logger)
	{
		_appDataAccess = appDataAccess;
		_locker = locker;
	}

	public override void SetTag(Screenshot<ClassifierAsset> screenshot, ClassifierTag? tag)
	{
		lock (_locker)
		{
			base.SetTag(screenshot, tag);
			_appDataAccess.SetDataChanged();
		}
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _locker;
}