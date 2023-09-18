using Avalonia.ReactiveUI;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

namespace SightKeeper.Avalonia.Views;

public sealed partial class ProfilesTab : ReactiveUserControl<ProfilesViewModel>
{
    public ProfilesTab()
    {
        InitializeComponent();
    }
}