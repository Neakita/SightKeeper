using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class DataSetViewModel : ViewModel
{
    private static readonly string[] Properties =
    {
        nameof(Name),
        nameof(Description),
        nameof(Game),
        nameof(Resolution)
    };
    
    public DataSet DataSet { get; }

    public string Name => DataSet.Name;
    public string Description => DataSet.Description;
    public Game? Game => DataSet.Game;
    public Resolution Resolution => DataSet.Resolution;

    public DataSetViewModel(DataSet dataSet)
    {
        DataSet = dataSet;
    }

    public void NotifyChanges() => OnPropertiesChanged(Properties);

    public override string ToString() => Name;
}