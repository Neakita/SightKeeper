using System.Collections.Generic;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	ImageSetSelectionDataContext ImageSetSelection { get; }
	DataSetSelectionDataContext DataSetSelection { get; }
	IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions { get; }
	object? AdditionalTooling { get; }
}