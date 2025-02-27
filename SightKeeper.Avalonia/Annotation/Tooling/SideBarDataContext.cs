using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.ImageSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	ImageSetViewModel? SelectedImageSet { get; set; }
	ScreenCapturingSettingsViewModel ScreenCapturingSettings { get; }
	IObservable<ushort> PendingImagesCount { get; }
	ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }
	DataSetViewModel? SelectedDataSet { get; set; }
	object? AdditionalTooling { get; }
}