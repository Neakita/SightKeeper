using System;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface AnnotationSideBarComponent : SideBarDataContext
{
	IObservable<ScreenshotsLibraryViewModel?> SelectedScreenshotsLibraryChanged { get; }
	IObservable<DataSetViewModel?> SelectedDataSetChanged { get; }
	IObservable<object?> AdditionalToolingChanged { get; }
}