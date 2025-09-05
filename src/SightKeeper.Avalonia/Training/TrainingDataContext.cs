using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Training;

public interface TrainingDataContext
{
	IReadOnlyCollection<DataSet<Asset>> DataSets { get; }
	DataSet<Asset>? DataSet { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	ICommand StartTrainingCommand { get; }
	ICommand StopTrainingCommand { get; }
}