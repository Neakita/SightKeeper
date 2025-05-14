using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataClassifierAnnotator : ClassifierAnnotator
{
	public AppDataClassifierAnnotator(AppDataAccess dataAccess, [Tag(typeof(AppData))] Lock appDataLock, AssetsMaker assetsMaker) : base(assetsMaker)
	{
		_dataAccess = dataAccess;
		_appDataLock = appDataLock;
	}

	public override void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image, Tag tag)
	{
		lock (_appDataLock)
			base.SetTag(assetsLibrary, image, tag);
		_dataAccess.SetDataChanged();
	}

	public override void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image)
	{
		lock (_appDataLock)
			base.DeleteAsset(assetsLibrary, image);
		_dataAccess.SetDataChanged();
	}

	private readonly AppDataAccess _dataAccess;
	private readonly Lock _appDataLock;
}