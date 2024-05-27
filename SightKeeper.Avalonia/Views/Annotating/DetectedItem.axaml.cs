using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class DetectedItem : ReactiveUserControl<DetectedItemViewModel>
{
    public DetectedItem()
    {
        InitializeComponent();
    }
}