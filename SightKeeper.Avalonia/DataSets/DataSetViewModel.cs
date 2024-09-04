using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DataSetViewModel : ViewModel
{
    public DataSet DataSet { get; }
    public string Name => DataSet.Name;
    public string Description => DataSet.Description;
    public Game? Game => DataSet.Game;
    public ushort Resolution => DataSet.Resolution;

    public DataSetViewModel(DataSet dataSet)
    {
	    DataSet = dataSet;
    }
}