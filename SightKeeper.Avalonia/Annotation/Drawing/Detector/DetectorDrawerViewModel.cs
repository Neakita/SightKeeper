using System;
using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.Drawing.Detector;

internal sealed class DetectorDrawerViewModel : DrawerViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public override ScreenshotViewModel<DetectorAssetViewModel, DetectorAsset>? Screenshot { get; set; }
	public override IReadOnlyCollection<DetectorItemViewModel> Items => throw new NotImplementedException();
}