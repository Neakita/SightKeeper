using SightKeeper.Application.Config;
using SightKeeper.Avalonia.ViewModels.Dialogs;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeConfigEditorViewModel : ConfigEditorViewModel
{
    public FakeConfigEditorViewModel() : base(new ConfigDataValidator())
    {
        Name = "Test config";
        Content = "Some content {Width}";
    }
}