using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;

namespace SightKeeper.Avalonia.Views.Annotating;

internal sealed partial class AutoAnnotation : ReactiveUserControl<AutoAnnotationViewModel>
{
    public AutoAnnotation()
    {
        InitializeComponent();
    }
}