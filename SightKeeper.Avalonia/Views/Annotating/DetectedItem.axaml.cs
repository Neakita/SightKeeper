using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating;

namespace SightKeeper.Avalonia.Views.Annotating;

public partial class DetectedItem : ReactiveUserControl<DetectedItemViewModel>
{
    public DetectedItem()
    {
        InitializeComponent();
    }
}