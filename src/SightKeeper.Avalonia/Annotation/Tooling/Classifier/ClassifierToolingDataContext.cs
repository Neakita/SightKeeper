using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.Classifier;

public interface ClassifierToolingDataContext
{
	bool IsEnabled { get; }
	IEnumerable<TagDataContext> Tags { get; }
	TagDataContext? SelectedTag { get; set; }
}