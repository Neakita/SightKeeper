using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Dialogs;

namespace SightKeeper.Avalonia.Views.Dialogs;

internal partial class WeightsEditor : ReactiveUserControl<WeightsEditorViewModel>
{
    public WeightsEditor()
    {
        InitializeComponent();
    }
}