using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataAnnotator : AssetsMaker, AssetsDeleter, ObservableAnnotator
{
	public IObservable<Image> AssetsChanged => _assetsChanged;

	public AppDataAnnotator([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, Image image)
	{
		TAsset asset;
		lock (_appDataLock)
			asset = assetsOwner.MakeAsset(image);
		_appDataAccess.SetDataChanged();
		_assetsChanged.OnNext(image);
		return asset;
	}

	public void DeleteAsset(AssetsOwner<Asset> assetsOwner, Image image)
	{
		lock (_appDataLock)
			assetsOwner.DeleteAsset(image);
		_appDataAccess.SetDataChanged();
		_assetsChanged.OnNext(image);
	}

	private readonly Subject<Image> _assetsChanged = new();
	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;
}