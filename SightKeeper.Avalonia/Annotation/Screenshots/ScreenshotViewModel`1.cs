using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract class ScreenshotViewModel<TAssetViewModel> : ScreenshotViewModel
	where TAssetViewModel : AssetViewModel
{
	public abstract override TAssetViewModel? Asset { get; }
}