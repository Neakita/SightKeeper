using System.Collections.Generic;
using SightKeeper.Avalonia.DataSets;

namespace SightKeeper.Avalonia.Annotation.Tooling;

public interface DataSetSelectionDataContext
{
	IReadOnlyCollection<DataSetDataContext> DataSets { get; }
	DataSetDataContext? SelectedDataSet { get; set; }
}