using System.Collections.Generic;

namespace SightKeeper.Avalonia.DataSets.Dialogs;

public interface DataSetTypePickerDataContext
{
	IReadOnlyCollection<DataSetTypeDataContext> Types { get; }
	DataSetTypeDataContext SelectedType { get; set; }
}