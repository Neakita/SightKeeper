using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Dialogs;

namespace SightKeeper.Avalonia.Views.Dialogs;

public partial class WeightsEditor : ReactiveUserControl<WeightsEditorViewModel>
{
    public WeightsEditor()
    {
        InitializeComponent();
    }
}