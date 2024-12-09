using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel<TAssetViewModel, TAsset> : DataSetContextViewModel<TAssetViewModel>
	where TAssetViewModel : AssetViewModel<TAsset>, AssetViewModelFactory<TAssetViewModel, TAsset>
	where TAsset : Asset, AssetsFactory<TAsset>, AssetsDestroyer<TAsset>
{
	public abstract override ScreenshotsViewModel<TAssetViewModel, TAsset> Screenshots { get; }
	public abstract override ToolBarViewModel<TAssetViewModel, TAsset> ToolBar { get; }
}