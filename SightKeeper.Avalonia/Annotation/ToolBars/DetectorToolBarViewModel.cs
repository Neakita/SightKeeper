using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

public sealed partial class DetectorToolBarViewModel : ToolBarViewModel
{
	[ObservableProperty] public partial IReadOnlyCollection<Tag> Tags { get; set; }
	[ObservableProperty] public partial Tag? Tag { get; set; }

	public DetectorToolBarViewModel(IReadOnlyCollection<Tag> tags)
	{
		Tags = tags;
	}

}