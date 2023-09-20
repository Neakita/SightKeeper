using System.Collections.Generic;
using System.Windows.Input;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public interface IProfilesViewModel
{
    IReadOnlyCollection<ProfileViewModel> Profiles { get; }
    ICommand CreateProfileCommand { get; }
    ICommand EditProfileCommand { get; }
    ICommand LaunchProfileCommand { get; }
    ICommand StopProfileCommand { get; }
}