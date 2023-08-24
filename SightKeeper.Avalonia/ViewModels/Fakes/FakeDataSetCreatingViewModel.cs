namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeDataSetCreatingViewModel : Dialogs.DataSetCreatingViewModel
{
    public FakeDataSetCreatingViewModel() : base(null, null)
    {
        Name = "Some data set";
    }
}