using System;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AnnotationSideBarComponent : SideBarDataContext
{
	IObservable<DataSetViewModel?> SelectedDataSetChanged { get; }
	IObservable<object?> AdditionalToolingChanged { get; }
}