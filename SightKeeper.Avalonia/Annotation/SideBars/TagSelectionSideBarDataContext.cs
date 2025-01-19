using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.SideBars;

public interface TagSelectionSideBarDataContext
{
	bool IsEnabled { get; }
	IReadOnlyCollection<Tag> Tags { get; }
	Tag? SelectedTag { get; set; }
}