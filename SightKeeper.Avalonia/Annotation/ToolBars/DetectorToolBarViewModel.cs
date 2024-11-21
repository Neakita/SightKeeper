using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed partial class DetectorToolBarViewModel : ToolBarViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public IReadOnlyCollection<DetectorTag> Tags { get; }

	public DetectorToolBarViewModel(IReadOnlyCollection<DetectorTag> tags)
	{
		Tags = tags;
	}

	[ObservableProperty] private DetectorTag? _tag;
}