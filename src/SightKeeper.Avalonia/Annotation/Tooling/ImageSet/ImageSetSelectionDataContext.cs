using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling.ImageSet;

public interface ImageSetSelectionDataContext
{
	IReadOnlyCollection<ImageSetDataContext> ImageSets { get; }
	ImageSetDataContext? SelectedImageSet { get; set; }
}