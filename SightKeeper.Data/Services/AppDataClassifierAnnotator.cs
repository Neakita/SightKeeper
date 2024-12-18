using SightKeeper.Application;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Data.Services;

public sealed class AppDataClassifierAnnotator : ClassifierAnnotator
{
	public AppDataClassifierAnnotator(AppDataAccess dataAccess, [Tag(typeof(AppData))] Lock appDataLock)
	{
		_dataAccess = dataAccess;
		_appDataLock = appDataLock;
	}

	public override void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot, Tag tag)
	{
		lock (_appDataLock)
			base.SetTag(assetsLibrary, screenshot, tag);
		_dataAccess.SetDataChanged();
	}

	public override void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Screenshot screenshot)
	{
		lock (_appDataLock)
			base.DeleteAsset(assetsLibrary, screenshot);
		_dataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _dataAccess;
	private readonly Lock _appDataLock;
}