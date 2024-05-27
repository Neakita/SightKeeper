using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

internal partial class ViewSettings : ReactiveUserControl<ViewSettingsViewModel>
{
    public ViewSettings()
    {
        InitializeComponent();
    }
}