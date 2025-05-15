using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Classifier;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataClassifierAnnotator : ClassifierAnnotator
{
	public AppDataClassifierAnnotator(ChangeListener changeListener, [Tag(typeof(AppData))] Lock appDataLock, AssetsMaker assetsMaker) : base(assetsMaker)
	{
		_changeListener = changeListener;
		_appDataLock = appDataLock;
	}

	public override void SetTag(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image, Tag tag)
	{
		lock (_appDataLock)
			base.SetTag(assetsLibrary, image, tag);
		_changeListener.SetDataChanged();
	}

	public override void DeleteAsset(AssetsLibrary<ClassifierAsset> assetsLibrary, Image image)
	{
		lock (_appDataLock)
			base.DeleteAsset(assetsLibrary, image);
		_changeListener.SetDataChanged();
	}

	private readonly ChangeListener _changeListener;
	private readonly Lock _appDataLock;
}