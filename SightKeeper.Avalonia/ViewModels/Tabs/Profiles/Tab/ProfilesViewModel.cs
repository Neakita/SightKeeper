using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed partial class ProfilesViewModel : IProfilesViewModel
{
    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }

    public ProfilesViewModel(ProfilesListViewModel profilesList)
    {
        Profiles = profilesList.ProfileViewModels;
    }

    ICommand IProfilesViewModel.CreateProfileCommand => CreateProfileCommand;
    [RelayCommand]
    private async Task CreateProfile()
    {
        
    }

    ICommand IProfilesViewModel.EditProfileCommand => EditProfileCommand;
    [RelayCommand]
    private async Task EditProfile(ProfileViewModel profile)
    {
        
    }
}