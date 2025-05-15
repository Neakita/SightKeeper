using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Application.Annotation;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.Services;

public sealed class AppDataAnnotator : AssetsMaker, AssetsDeleter, ObservableAnnotator
{
	public IObservable<Image> AssetsChanged => _assetsChanged;

	public AppDataAnnotator([Tag(typeof(AppData))] Lock appDataLock, ChangeListener changeListener)
	{
		_appDataLock = appDataLock;
		_changeListener = changeListener;
	}

	public TAsset MakeAsset<TAsset>(AssetsOwner<TAsset> assetsOwner, Image image)
	{
		TAsset asset;
		lock (_appDataLock)
			asset = assetsOwner.MakeAsset(image);
		_changeListener.SetDataChanged();
		_assetsChanged.OnNext(image);
		return asset;
	}

	public void DeleteAsset(AssetsOwner<Asset> assetsOwner, Image image)
	{
		lock (_appDataLock)
			assetsOwner.DeleteAsset(image);
		_changeListener.SetDataChanged();
		_assetsChanged.OnNext(image);
	}

	private readonly Subject<Image> _assetsChanged = new();
	private readonly Lock _appDataLock;
	private readonly ChangeListener _changeListener;
}