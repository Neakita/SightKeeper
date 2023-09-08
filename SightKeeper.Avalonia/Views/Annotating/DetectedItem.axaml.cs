using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public sealed partial class DetectedItem : ReactiveUserControl<DetectedItemViewModel>
{
    public DetectedItem()
    {
        InitializeComponent();
    }
}