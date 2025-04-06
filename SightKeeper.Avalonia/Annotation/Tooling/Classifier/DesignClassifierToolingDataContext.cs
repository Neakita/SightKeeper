using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.Classifier;

internal sealed class DesignClassifierToolingDataContext : ClassifierToolingDataContext
{
	public bool IsEnabled => true;

	public IEnumerable<TagDataContext> Tags =>
	[
		new DesignTagDataContext("Shoot"),
		new DesignTagDataContext("Don't Shoot")
	];

	public TagDataContext? SelectedTag { get; set; }
}