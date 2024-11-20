using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract class ScreenshotViewModel<TAssetViewModel> : ScreenshotViewModel
	where TAssetViewModel : AssetViewModel
{
	public override abstract TAssetViewModel? Asset { get; }

	public ScreenshotViewModel(ScreenshotImageLoader imageLoader) : base(imageLoader)
	{
	}
}