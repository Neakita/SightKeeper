using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	IReadOnlyCollection<ImageSetDataContext> ImageSets { get; }
	ImageSetDataContext? SelectedImageSet { get; set; }
	DataSetSelectionDataContext DataSetSelection { get; }
	IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions { get; }
	object? AdditionalTooling { get; }
}