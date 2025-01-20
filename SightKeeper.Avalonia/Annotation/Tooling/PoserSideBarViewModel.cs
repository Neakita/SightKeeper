using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public sealed partial class PoserSideBarViewModel : ViewModel
{
	[ObservableProperty]
	public partial IReadOnlyCollection<PoserTag> Tags { get; set; } = ReadOnlyCollection<PoserTag>.Empty;
	[ObservableProperty] public partial PoserTag? SelectedTag { get; set; }
	
	[ObservableProperty]
	public partial IReadOnlyCollection<Tag> KeyPointTags { get; set; } = ReadOnlyCollection<Tag>.Empty;
	[ObservableProperty] public partial Tag? SelectedKeyPointTag { get; set; }
}