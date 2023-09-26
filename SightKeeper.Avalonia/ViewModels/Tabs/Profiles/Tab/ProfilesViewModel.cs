using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Scoring;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed partial class ProfilesViewModel : ViewModel, IProfilesViewModel
{
    public bool MakeScreenshots
    {
        get => _profileRunner.MakeScreenshots;
        set => SetProperty(_profileRunner.MakeScreenshots, value, newValue => _profileRunner.MakeScreenshots = newValue);
    }
    public float MinimumProbability
    {
        get => _profileRunner.MinimumProbability;
        set => SetProperty(_profileRunner.MinimumProbability, value, newValue =>
        {
            if (MaximumProbability <= newValue)
                MaximumProbability = newValue + 0.1f;
            _profileRunner.MinimumProbability = newValue;
        });
    }
    public float MaximumProbability
    {
        get => _profileRunner.MaximumProbability;
        set => SetProperty(_profileRunner.MaximumProbability, value, newValue =>
        {
            if (MinimumProbability >= newValue)
                MinimumProbability = newValue - 0.1f;
            _profileRunner.MaximumProbability = newValue;
        });
    }

    public byte MaximumFPS
    {
        get => _profileRunner.MaximumFPS;
        set => SetProperty(_profileRunner.MaximumFPS, value, newValue => _profileRunner.MaximumFPS = newValue);
    }

    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }

    public ProfilesViewModel(ProfilesListViewModel profilesList, ILifetimeScope scope, ProfileCreator profileCreator, ProfileEditor profileEditor, ProfileRunner profileRunner, ProfilesDataAccess profilesDataAccess)
    {
        _scope = scope;
        _profileCreator = profileCreator;
        _profileEditor = profileEditor;
        _profileRunner = profileRunner;
        _profilesDataAccess = profilesDataAccess;
        Profiles = profilesList.ProfileViewModels;
    }

    private readonly ILifetimeScope _scope;
    private readonly ProfileCreator _profileCreator;
    private readonly ProfileEditor _profileEditor;
    private readonly ProfileRunner _profileRunner;
    private readonly ProfilesDataAccess _profilesDataAccess;

    ICommand IProfilesViewModel.CreateProfileCommand => CreateProfileCommand;
    [RelayCommand]
    private async Task CreateProfile()
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<NewProfileEditorViewModel>();
        var result = await viewModel.ShowDialog(this);
        if (result == ProfileEditorResult.Apply)
        {
            Guard.IsNotNull(viewModel.Weights);
            NewProfileDataDTO data = new(viewModel.Name, viewModel.Description, viewModel.DetectionThreshold, viewModel.MouseSensitivity, viewModel.PostProcessDelay, viewModel.Weights, viewModel.ItemClasses);
            await _profileCreator.CreateProfile(data);
        }
    }

    ICommand IProfilesViewModel.EditProfileCommand => EditProfileCommand;

    [RelayCommand]
    private async Task EditProfile(ProfileViewModel profileViewModel)
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<ExistingProfileEditorViewModel>();
        viewModel.SetData(profileViewModel.Profile);
        var result = await viewModel.ShowDialog(this);
        if (result == ProfileEditorResult.Apply)
        {
            Guard.IsNotNull(viewModel.Weights);
            EditedProfileDataDTO data = new(profileViewModel.Profile, viewModel.Name, viewModel.Description, viewModel.DetectionThreshold, viewModel.MouseSensitivity, viewModel.PostProcessDelay, viewModel.Weights, viewModel.ItemClasses);
            await _profileEditor.ApplyChanges(data);
        }
        else if (result == ProfileEditorResult.Delete)
        {
            Guard.IsNotNull(viewModel.Profile);
            await _profilesDataAccess.RemoveProfile(viewModel.Profile);
        }
    }

    ICommand IProfilesViewModel.LaunchProfileCommand => LaunchProfileCommand;
    [RelayCommand(CanExecute = nameof(CanLaunchProfile))]
    private void LaunchProfile(ProfileViewModel profileViewModel)
    {
        Guard.IsFalse(_runningProfile == profileViewModel);
        if (_runningProfile != null)
            _profileRunner.Stop();
        _profileRunner.Run(profileViewModel.Profile);
        _runningProfile = profileViewModel;
        LaunchProfileCommand.NotifyCanExecuteChanged();
        StopProfileCommand.NotifyCanExecuteChanged();
    }
    private bool CanLaunchProfile(ProfileViewModel profileViewModel) => _runningProfile != profileViewModel;
    
    ICommand IProfilesViewModel.StopProfileCommand => StopProfileCommand;
    [RelayCommand(CanExecute = nameof(CanStopProfile))]
    private void StopProfile(ProfileViewModel profile)
    {
        Guard.IsNotNull(_runningProfile);
        _profileRunner.Stop();
        _runningProfile = null;
        LaunchProfileCommand.NotifyCanExecuteChanged();
        StopProfileCommand.NotifyCanExecuteChanged();
    }

    private bool CanStopProfile(ProfileViewModel profile) => profile == _runningProfile;

    private ProfileViewModel? _runningProfile;
}