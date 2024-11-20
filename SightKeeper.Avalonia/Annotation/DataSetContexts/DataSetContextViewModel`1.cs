using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Drawing;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ToolBars;

namespace SightKeeper.Avalonia.Annotation.DataSetContexts;

internal abstract class DataSetContextViewModel<TAssetViewModel> : DataSetContextViewModel
	where TAssetViewModel : AssetViewModel
{
	public abstract override ScreenshotsViewModel<TAssetViewModel> Screenshots { get; }
	public abstract override ToolBarViewModel<TAssetViewModel> ToolBar { get; }
	public abstract override DrawerViewModel<TAssetViewModel>? Drawer { get; }
}