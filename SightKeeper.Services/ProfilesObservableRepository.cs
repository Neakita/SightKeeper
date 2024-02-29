using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using Serilog;
using SightKeeper.Commons;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class ProfilesObservableRepository : IDisposable
{
    public IObservableList<Profile> Profiles => _profiles;

    public ProfilesObservableRepository(ProfilesDataAccess profilesDataAccess)
    {
        LoadProfiles(profilesDataAccess);
        profilesDataAccess.ProfileAdded
            .Subscribe(AddProfile)
            .DisposeWithEx(_disposable);
        profilesDataAccess.ProfileRemoved
            .Subscribe(RemoveProfile)
            .DisposeWithEx(_disposable);
    }

    public void Dispose()
    {
        _disposable.Dispose();
        _profiles.Dispose();
    }

    private readonly CompositeDisposable _disposable = new();
    private readonly SourceList<Profile> _profiles = new();

    private void LoadProfiles(ProfilesDataAccess profilesDataAccess)
    {
        _profiles.AddRange(profilesDataAccess.LoadProfiles());
    }
    
    private void AddProfile(Profile profile)
    {
        Log.Debug("Profile Id: {Id}", profile.Id);
        _profiles.Add(profile);
    }

    private void RemoveProfile(Profile profile) => Guard.IsTrue(_profiles.Remove(profile));
}