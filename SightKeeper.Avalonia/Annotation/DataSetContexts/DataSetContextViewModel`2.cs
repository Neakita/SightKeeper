using System;
using System.ComponentModel;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel<TAssetViewModel, TAsset> : DataSetContextViewModel<TAssetViewModel>, IDisposable
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public abstract override ScreenshotsViewModel<TAssetViewModel, TAsset> Screenshots { get; }
	public abstract override ToolBarViewModel<TAssetViewModel, TAsset> ToolBar { get; }

	private void OnScreenshotsPropertyChanged(object? sender, PropertyChangedEventArgs args)
	{
		if (args.PropertyName == nameof(Screenshots.SelectedScreenshot))
			ToolBar.Screenshot = (ScreenshotViewModel<TAssetViewModel, TAsset>?)Screenshots.SelectedScreenshot;
	}

	public void Dispose()
	{
		Screenshots.PropertyChanged -= OnScreenshotsPropertyChanged;
	}

	protected void Initialize()
	{
		Screenshots.PropertyChanged += OnScreenshotsPropertyChanged;
	}
}