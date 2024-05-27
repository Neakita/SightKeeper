using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Tab;

internal sealed class ProfilesListViewModel : IDisposable
{
    public ReadOnlyCollection<ProfileViewModel> ProfileViewModels { get; }

    public ProfilesListViewModel(ProfilesObservableRepository profilesObservableRepository, ProfileEditor profileEditor)
    {
        profilesObservableRepository.Profiles.Connect()
            .Transform(profile => new ProfileViewModel(profile))
            .AddKey(profile => profile.Profile)
            .PopulateInto(_profileViewModels)
            .DisposeWith(_constructorDisposables);
        Guard.IsNotNull(SynchronizationContext.Current);
        _profileViewModels.Connect()
            .ObserveOn(SynchronizationContext.Current)
            .Bind(out var profileViewModels)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        ProfileViewModels = profileViewModels;
        profileEditor.ProfileEdited.Subscribe(OnProfileEdited).DisposeWith(_constructorDisposables);
    }

    private void OnProfileEdited(Profile editedProfile) =>
        _profileViewModels.Lookup(editedProfile).Value.NotifyChanges();

    public void Dispose()
    {
        _constructorDisposables.Dispose();
        _profileViewModels.Dispose();
    }

    private readonly CompositeDisposable _constructorDisposables = new();
    private readonly SourceCache<ProfileViewModel, Profile> _profileViewModels =
        new(profileViewModel => profileViewModel.Profile);
}