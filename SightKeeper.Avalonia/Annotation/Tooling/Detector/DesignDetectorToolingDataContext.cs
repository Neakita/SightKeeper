using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.Detector;

internal sealed class DesignDetectorToolingDataContext : DetectorToolingDataContext
{
	public IEnumerable<TagDataContext> Tags =>
	[
		new DesignTagDataContext("Ally"),
		new DesignTagDataContext("Enemy")
	];

	public TagDataContext? SelectedTag { get; set; }
}