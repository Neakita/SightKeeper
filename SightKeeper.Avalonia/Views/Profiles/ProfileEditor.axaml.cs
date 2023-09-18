using Avalonia.Controls;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

namespace SightKeeper.Avalonia.Views;

public sealed partial class ProfileEditor : UserControl, IViewFor<NewProfileEditorViewModel>
{
    public NewProfileEditorViewModel? ViewModel { get; set; }
    
    public ProfileEditor()
    {
        InitializeComponent();
    }

    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (NewProfileEditorViewModel?)value;
    }
}