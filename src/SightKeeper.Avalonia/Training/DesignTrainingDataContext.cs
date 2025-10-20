using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Avalonia.Training;

internal sealed class DesignTrainingDataContext : TrainingDataContext
{
	public IReadOnlyCollection<DataSet<Tag, Asset>> DataSets => ReadOnlyCollection<DataSet<Tag, Asset>>.Empty;
	public DataSet<Tag, Asset>? DataSet { get; set; }
	public ushort Width { get; set; } = 320;
	public ushort Height { get; set; } = 320;
	public bool IsTraining => false;
	public ICommand StartTrainingCommand => CommandStubs.Enabled;
	public ICommand StopTrainingCommand => CommandStubs.Disabled;
}