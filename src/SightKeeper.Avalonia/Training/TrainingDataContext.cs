using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Training;

public interface TrainingDataContext
{
	IReadOnlyCollection<DataSet<Tag, Asset>> DataSets { get; }
	DataSet<Tag, Asset>? DataSet { get; set; }
	ushort Width { get; set; }
	ushort Height { get; set; }
	bool IsTraining { get; }
	ICommand StartTrainingCommand { get; }
	ICommand StopTrainingCommand { get; }
	IEnumerable<string> LogLines { get; }
}