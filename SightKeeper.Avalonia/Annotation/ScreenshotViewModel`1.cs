using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation;

internal sealed class ScreenshotViewModel<TAssetViewModel> : ScreenshotViewModel
	where TAssetViewModel : AssetViewModel
{
	public TAssetViewModel? Asset { get; private set; }

	public ScreenshotViewModel(Screenshot screenshot, ScreenshotImageLoader imageLoader) : base(screenshot, imageLoader)
	{
	}
}