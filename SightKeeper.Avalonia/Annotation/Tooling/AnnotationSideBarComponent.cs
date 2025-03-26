using System;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AnnotationSideBarComponent : SideBarDataContext
{
	IObservable<ImageSetViewModel?> SelectedImageSetChanged { get; }
	IObservable<DataSetViewModel?> SelectedDataSetChanged { get; }
	IObservable<object?> AdditionalToolingChanged { get; }
}