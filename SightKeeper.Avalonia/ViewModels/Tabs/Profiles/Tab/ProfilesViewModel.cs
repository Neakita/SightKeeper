using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed partial class ProfilesViewModel : ViewModel, IProfilesViewModel
{
    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }

    public ProfilesViewModel(ProfilesListViewModel profilesList, ILifetimeScope scope, ProfileCreator profileCreator)
    {
        _scope = scope;
        _profileCreator = profileCreator;
        Profiles = profilesList.ProfileViewModels;
    }

    private readonly ILifetimeScope _scope;
    private readonly ProfileCreator _profileCreator;

    ICommand IProfilesViewModel.CreateProfileCommand => CreateProfileCommand;
    [RelayCommand]
    private async Task CreateProfile()
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<NewProfileEditorViewModel>();
        var applied = await viewModel.ShowDialog(this);
        if (!applied)
            return;
        Guard.IsNotNull(viewModel.Weights);
        NewProfileDataDTO data = new(viewModel.Name, viewModel.Description, viewModel.DetectionThreshold, viewModel.MouseSensitivity, viewModel.Weights, viewModel.ItemClasses);
        await _profileCreator.CreateProfile(data);
    }

    ICommand IProfilesViewModel.EditProfileCommand => EditProfileCommand;
    [RelayCommand]
    private async Task EditProfile(ProfileViewModel profile)
    {
        
    }
}