﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Scoring;
using SightKeeper.Avalonia.Extensions;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles;

public sealed partial class ProfilesViewModel : ViewModel, IProfilesViewModel
{
    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }

    public ProfilesViewModel(ProfilesListViewModel profilesList, ILifetimeScope scope, ProfileCreator profileCreator, ProfileEditor profileEditor, ProfileRunner profileRunner)
    {
        _scope = scope;
        _profileCreator = profileCreator;
        _profileEditor = profileEditor;
        _profileRunner = profileRunner;
        Profiles = profilesList.ProfileViewModels;
    }

    private readonly ILifetimeScope _scope;
    private readonly ProfileCreator _profileCreator;
    private readonly ProfileEditor _profileEditor;
    private readonly ProfileRunner _profileRunner;

    ICommand IProfilesViewModel.CreateProfileCommand => CreateProfileCommand;
    [RelayCommand]
    private async Task CreateProfile()
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<NewProfileEditorViewModel>();
        var isApplied = await viewModel.ShowDialog(this);
        if (!isApplied)
            return;
        Guard.IsNotNull(viewModel.Weights);
        NewProfileDataDTO data = new(viewModel.Name, viewModel.Description, viewModel.DetectionThreshold, viewModel.MouseSensitivity, viewModel.Weights, viewModel.ItemClasses);
        await _profileCreator.CreateProfile(data);
    }

    ICommand IProfilesViewModel.EditProfileCommand => EditProfileCommand;

    [RelayCommand]
    private async Task EditProfile(ProfileViewModel profileViewModel)
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<ExistingProfileEditorViewModel>();
        viewModel.SetData(profileViewModel.Profile);
        var isApplied = await viewModel.ShowDialog(this);
        if (!isApplied)
            return;
        Guard.IsNotNull(viewModel.Weights);
        EditedProfileDataDTO data = new(profileViewModel.Profile, viewModel.Name, viewModel.Description, viewModel.DetectionThreshold, viewModel.MouseSensitivity, viewModel.Weights, viewModel.ItemClasses);
        await _profileEditor.ApplyChanges(data);
    }

    ICommand IProfilesViewModel.LaunchProfileCommand => LaunchProfileCommand;
    [RelayCommand(CanExecute = nameof(CanLaunchProfile))]
    private async Task LaunchProfile(ProfileViewModel profileViewModel)
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
    private async Task StopProfile(ProfileViewModel profile)
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