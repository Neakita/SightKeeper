using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

internal sealed partial class DetectorToolBarViewModel : ToolBarViewModel
{
	public IReadOnlyCollection<Tag> Tags { get; }

	public DetectorToolBarViewModel(IReadOnlyCollection<Tag> tags)
	{
		Tags = tags;
	}

	[ObservableProperty] private Tag? _tag;
}