using Avalonia.Controls;
using ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

namespace SightKeeper.Avalonia.Views.Profiles;

internal sealed partial class ProfileEditor : UserControl, IViewFor<NewProfileEditorViewModel>, IViewFor<ExistingProfileEditorViewModel>
{
    private ExistingProfileEditorViewModel? _viewModel;
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

    ExistingProfileEditorViewModel? IViewFor<ExistingProfileEditorViewModel>.ViewModel
    {
        get => _viewModel;
        set => _viewModel = value;
    }
}