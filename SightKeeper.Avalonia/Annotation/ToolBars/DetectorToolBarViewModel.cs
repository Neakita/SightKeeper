using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

public sealed partial class DetectorToolBarViewModel : ToolBarViewModel
{
	[ObservableProperty] public partial IReadOnlyCollection<Tag> Tags { get; set; } = ReadOnlyCollection<Tag>.Empty;
	[ObservableProperty] public partial Tag? Tag { get; set; }
}