using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Assets;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract class ScreenshotsViewModel<TAssetViewModel> : ScreenshotsViewModel
	where TAssetViewModel : AssetViewModel
{
	public abstract override IReadOnlyCollection<ScreenshotViewModel<TAssetViewModel>> Screenshots { get; }

	protected ScreenshotsViewModel(ScreenshotImageLoader imageLoader) : base(imageLoader)
	{
	}
}