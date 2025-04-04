using System.Collections.Generic;
using SightKeeper.Avalonia.DataSets;
using Vibrance;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface SideBarDataContext
{
	IReadOnlyCollection<ImageSetViewModel> ImageSets { get; }
	ImageSetViewModel? SelectedImageSet { get; set; }
	ReadOnlyObservableList<DataSetViewModel> DataSets { get; }
	DataSetViewModel? SelectedDataSet { get; set; }
	object? AdditionalTooling { get; }
}