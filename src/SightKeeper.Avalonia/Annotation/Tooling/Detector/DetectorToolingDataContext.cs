using System.Collections.Generic;
using SightKeeper.Avalonia.Annotation.Tooling.Tags;

namespace SightKeeper.Avalonia.Annotation.Tooling.Detector;

public interface DetectorToolingDataContext
{
	IEnumerable<TagDataContext> Tags { get; }
	TagDataContext? SelectedTag { get; set; }
}