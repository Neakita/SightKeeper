using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Application.Prediction;
using SightKeeper.Application.Prediction.Handling;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Tab;

internal partial class ProfilesViewModel : ViewModel
{
    public bool MakeScreenshots
    {
        get => _screenshotingParameters.MakeScreenshots;
        set => SetProperty(_screenshotingParameters.MakeScreenshots, value, newValue => _screenshotingParameters.MakeScreenshots = newValue);
    }
    public float MinimumProbability
    {
        get => _screenshotingParameters.MinimumProbability;
        set => SetProperty(_screenshotingParameters.MinimumProbability, value, newValue =>
        {
            if (MaximumProbability <= newValue)
                MaximumProbability = newValue + 0.1f;
            _screenshotingParameters.MinimumProbability = newValue;
        });
    }
    public float MaximumProbability
    {
        get => _screenshotingParameters.MaximumProbability;
        set => SetProperty(_screenshotingParameters.MaximumProbability, value, newValue =>
        {
            if (MinimumProbability >= newValue)
                MinimumProbability = newValue - 0.1f;
            _screenshotingParameters.MaximumProbability = newValue;
        });
    }

    public byte MaximumFPS
    {
        get => _screenshotingParameters.MaximumFPS;
        set => SetProperty(_screenshotingParameters.MaximumFPS, value, newValue => _screenshotingParameters.MaximumFPS = newValue);
    }

    public IReadOnlyCollection<ProfileViewModel> Profiles { get; }

    public ProfilesViewModel(
        ProfilesListViewModel profilesList,
        ILifetimeScope scope,
        ProfileCreator profileCreator,
        ProfileEditor profileEditor,
        ProfileRunner profileRunner,
        ProfilesDataAccess profilesDataAccess,
        DetectionScreenshotingParameters screenshotingParameters,
        DialogManager dialogManager)
    {
        _scope = scope;
        _profileCreator = profileCreator;
        _profileEditor = profileEditor;
        _profileRunner = profileRunner;
        _profilesDataAccess = profilesDataAccess;
        _screenshotingParameters = screenshotingParameters;
        _dialogManager = dialogManager;
        Profiles = profilesList.ProfileViewModels;
    }

    private readonly ILifetimeScope _scope;
    private readonly ProfileCreator _profileCreator;
    private readonly ProfileEditor _profileEditor;
    private readonly ProfileRunner _profileRunner;
    private readonly ProfilesDataAccess _profilesDataAccess;
    private readonly DetectionScreenshotingParameters _screenshotingParameters;
    private readonly DialogManager _dialogManager;

    [RelayCommand]
    private async Task CreateProfile()
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<NewProfileEditorViewModel>();
        var result = await _dialogManager.ShowDialogAsync(viewModel);
        if (result == ProfileEditorResult.Apply)
        {
            Guard.IsNotNull(viewModel.Weights);
            NewProfileDataDTO data = new(
                viewModel.Name,
                viewModel.Description,
                viewModel.DetectionThreshold,
                viewModel.MouseSensitivity,
                viewModel.PostProcessDelay,
                viewModel.IsPreemptionEnabled,
                viewModel.PreemptionHorizontalFactor,
                viewModel.PreemptionVerticalFactor,
                viewModel.IsPreemptionStabilizationEnabled,
                viewModel.PreemptionStabilizationBufferSize,
                viewModel.PreemptionStabilizationMethod,
                viewModel.Weights,
                viewModel.Tags);
            _profileCreator.CreateProfile(data);
        }
    }

    [RelayCommand]
    private async Task EditProfile(ProfileViewModel profileViewModel)
    {
        await using var scope = _scope.BeginLifetimeScope();
        var viewModel = scope.Resolve<ExistingProfileEditorViewModel>();
        viewModel.SetData(profileViewModel.Profile);
        var result = await _dialogManager.ShowDialogAsync(viewModel);
        if (result == ProfileEditorResult.Apply)
        {
            Guard.IsNotNull(viewModel.Weights);
            EditedProfileDataDTO data = new(
                profileViewModel.Profile,
                viewModel.Name,
                viewModel.Description,
                viewModel.DetectionThreshold,
                viewModel.MouseSensitivity,
                viewModel.PostProcessDelay,
                viewModel.IsPreemptionEnabled,
                viewModel.PreemptionHorizontalFactor,
                viewModel.PreemptionVerticalFactor,
                viewModel.IsPreemptionStabilizationEnabled,
                viewModel.PreemptionStabilizationBufferSize,
                viewModel.PreemptionStabilizationMethod, viewModel.Weights, viewModel.Tags);
            _profileEditor.ApplyChanges(data);
        }
        else if (result == ProfileEditorResult.Delete)
        {
            Guard.IsNotNull(viewModel.Profile);
            _profilesDataAccess.RemoveProfile(viewModel.Profile);
        }
    }

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