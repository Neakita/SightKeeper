using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal abstract class ToolBarViewModel<TAssetViewModel, TAsset> : ToolBarViewModel<TAssetViewModel>
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public abstract ScreenshotViewModel<TAssetViewModel, TAsset>? Screenshot { get; set; }
}