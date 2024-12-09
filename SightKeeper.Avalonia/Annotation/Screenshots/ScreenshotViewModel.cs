using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal abstract class ScreenshotViewModel : ViewModel
{
	public abstract Screenshot Value { get; }
	public abstract AssetViewModel? Asset { get; }
}