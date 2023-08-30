using SightKeeper.Avalonia.ViewModels.Elements;

namespace SightKeeper.Avalonia.ViewModels.Annotating;

public sealed class SelectedDataSetViewModel : ValueViewModel<DataSetViewModel?>
{
    public SelectedDataSetViewModel() : base(null)
    {
    }
}