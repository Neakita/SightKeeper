using Avalonia.ReactiveUI;
using DetectedItemViewModel = SightKeeper.Avalonia.ViewModels.Annotating.Drawer.DetectedItemViewModel;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class DetectedItem : ReactiveUserControl<DetectedItemViewModel>
{
    public DetectedItem()
    {
        InitializeComponent();
    }
}