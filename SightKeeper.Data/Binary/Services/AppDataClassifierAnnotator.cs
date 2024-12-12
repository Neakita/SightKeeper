using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataClassifierAnnotator : ClassifierAnnotator
{
	public AppDataClassifierAnnotator(AppDataAccess dataAccess, AppDataEditingLock locker)
	{
		_dataAccess = dataAccess;
		_locker = locker;
	}

	public override void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot, Tag tag)
	{
		lock (_locker)
			base.SetTag(assetsLibrary, screenshot, tag);
		_dataAccess.SetDataChanged();
	}

	public override void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot)
	{
		lock (_locker)
			base.DeleteAsset(assetsLibrary, screenshot);
		_dataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _dataAccess;
	private readonly AppDataEditingLock _locker;
}