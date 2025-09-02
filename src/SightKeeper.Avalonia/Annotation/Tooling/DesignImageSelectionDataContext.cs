using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling;

internal sealed class DesignImageSelectionDataContext : ImageSetSelectionDataContext
{
	public IReadOnlyCollection<ImageSetDataContext> ImageSets =>
	[
		new DesignImageSetDataContext("Image Set 1"),
		new DesignImageSetDataContext("Image Set 2")
	];

	public ImageSetDataContext? SelectedImageSet { get; set; }
}