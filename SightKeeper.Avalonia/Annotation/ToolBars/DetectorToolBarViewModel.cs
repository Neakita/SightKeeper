using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed class DetectorToolBarViewModel : ToolBarViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override ScreenshotViewModel<DetectorAssetViewModel, DetectorAsset>? Screenshot { get; set; }
}