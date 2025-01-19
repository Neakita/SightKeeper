using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.ToolBars;

public interface TagSelectionToolBarDataContext
{
	bool IsEnabled { get; }
	IReadOnlyCollection<Tag> Tags { get; }
	Tag? SelectedTag { get; set; }
}