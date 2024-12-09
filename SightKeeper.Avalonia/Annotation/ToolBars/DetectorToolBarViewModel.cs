using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Avalonia.Annotation.Assets;
using SightKeeper.Domain.DataSets.Detector;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed partial class DetectorToolBarViewModel : ToolBarViewModel<DetectorAssetViewModel, DetectorAsset>
{
	public IReadOnlyCollection<Tag> Tags { get; }

	public DetectorToolBarViewModel(IReadOnlyCollection<Tag> tags)
	{
		Tags = tags;
	}

	[ObservableProperty] private Tag? _tag;
}