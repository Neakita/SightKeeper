using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.DataSets.Card;

namespace SightKeeper.Avalonia.DataSets;

internal interface DataSetsDataContext
{
	IReadOnlyCollection<DataSetCardDataContext> DataSets { get; }
	ICommand CreateDataSetCommand { get; }
	ICommand ImportDataSetCommand { get; }
}