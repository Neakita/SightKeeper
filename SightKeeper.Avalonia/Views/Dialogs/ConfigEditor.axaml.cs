using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Dialogs;

namespace SightKeeper.Avalonia.Views.Dialogs;

public sealed partial class ConfigEditor : UserControl, IViewFor<ConfigEditorViewModel>
{
    public ConfigEditor()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ConfigEditorViewModel?)value;
    }

    public ConfigEditorViewModel? ViewModel { get; set; }
}