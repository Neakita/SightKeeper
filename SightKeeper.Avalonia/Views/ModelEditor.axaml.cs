using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Views;

public sealed partial class ModelEditor : UserControl, IViewFor<ModelEditorViewModel>
{
    public ModelEditorViewModel? ViewModel { get; set; }
    
    public ModelEditor()
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
        set => ViewModel = (ModelEditorViewModel?)value;
    }
}