using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.DataSets;

internal interface IDataSetsViewModel
{
	IReadOnlyCollection<DataSetViewModel> DataSets { get; }
	ICommand CreateDataSetCommand { get; }
	ICommand EditDataSetCommand { get; }
	ICommand DeleteDataSetCommand { get; }
}