using System.Collections.Generic;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface TagSelectionToolingDataContext
{
	bool IsEnabled { get; }
	IReadOnlyCollection<Tag> Tags { get; }
	Tag? SelectedTag { get; set; }
}