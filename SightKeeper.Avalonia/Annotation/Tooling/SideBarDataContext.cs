using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ScreenshotsLibraries;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	IReadOnlyCollection<ScreenshotsLibraryViewModel> ScreenshotsLibraries { get; }
	ScreenshotsLibraryViewModel? SelectedScreenshotsLibrary { get; }
	ScreenshottingSettingsViewModel ScreenshottingSettings { get; }
	IObservable<ushort> PendingScreenshotsCount { get; }
	ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	DataSetViewModel? SelectedDataSet { get; set; }
	object? AdditionalTooling { get; }
}