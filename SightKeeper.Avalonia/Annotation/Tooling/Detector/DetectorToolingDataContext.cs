using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.Detector;

public interface DetectorToolingDataContext
{
	IEnumerable<TagDataContext> Tags { get; }
	TagDataContext? SelectedTag { get; set; }
}