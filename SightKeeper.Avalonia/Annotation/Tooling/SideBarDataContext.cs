using System.Collections.Generic;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	IReadOnlyCollection<ImageSetDataContext> ImageSets { get; }
	ImageSetDataContext? SelectedImageSet { get; set; }
	IReadOnlyCollection<DataSetDataContext> DataSets { get; }
	DataSetDataContext? SelectedDataSet { get; set; }
	IReadOnlyCollection<AnnotationButtonDefinition> ButtonDefinitions { get; }
	object? AdditionalTooling { get; }
}