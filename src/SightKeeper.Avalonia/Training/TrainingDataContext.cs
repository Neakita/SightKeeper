using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.DataSets;

namespace SightKeeper.Avalonia.Training;

public interface TrainingDataContext
{
	IReadOnlyCollection<DataSet> DataSets { get; }
	DataSet? DataSet { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ICommand StartTrainingCommand { get; }
	ICommand StopTrainingCommand { get; }
}