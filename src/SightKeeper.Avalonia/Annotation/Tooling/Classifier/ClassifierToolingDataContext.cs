using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Tooling.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Classifier;

public interface ClassifierToolingDataContext
{
	bool IsEnabled { get; }
	IEnumerable<TagDataContext> Tags { get; }
	TagDataContext? SelectedTag { get; set; }
}