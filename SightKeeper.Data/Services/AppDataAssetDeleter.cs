using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataAssetDeleter : AssetDeleter, ObservableAnnotator, IDisposable
{
	public IObservable<Image> AssetsChanged => _assetsChanged;

	public AppDataAssetDeleter([Tag(typeof(AppData))] Lock appDataLock, AppDataAccess appDataAccess)
	{
		_appDataLock = appDataLock;
		_appDataAccess = appDataAccess;
	}

	public void DeleteAsset(AssetsOwner<Asset> assetsOwner, Image image)
	{
		lock (_appDataLock)
			assetsOwner.DeleteAsset(image);
		_appDataAccess.SetDataChanged();
		_assetsChanged.OnNext(image);
	}

	public void Dispose()
	{
		_assetsChanged.Dispose();
	}

	private readonly Lock _appDataLock;
	private readonly AppDataAccess _appDataAccess;
	private readonly Subject<Image> _assetsChanged = new();
}