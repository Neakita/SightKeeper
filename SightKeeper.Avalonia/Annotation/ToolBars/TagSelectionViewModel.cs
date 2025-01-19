using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

public sealed partial class TagSelectionViewModel : ViewModel, TagSelectionToolBarDataContext
{
	public bool IsEnabled => true;
	[ObservableProperty] public partial IReadOnlyCollection<Tag> Tags { get; set; } = ReadOnlyCollection<Tag>.Empty;
	[ObservableProperty] public partial Tag? SelectedTag { get; set; }
}