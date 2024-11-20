using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal abstract class ToolBarViewModel<TAssetViewModel> : ToolBarViewModel where TAssetViewModel : AssetViewModel
{
}