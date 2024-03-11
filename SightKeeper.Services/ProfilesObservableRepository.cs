using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Services.Extensions;

namespace SightKeeper.Services;

public sealed class ProfilesObservableRepository : IDisposable
{
    public IObservableList<Profile> Profiles => _profiles;

    public ProfilesObservableRepository(ProfilesDataAccess profilesDataAccess)
    {
        LoadProfiles(profilesDataAccess);
        profilesDataAccess.ProfileAdded
            .Subscribe(AddProfile)
            .DisposeWith(_disposable);
        profilesDataAccess.ProfileRemoved
            .Subscribe(RemoveProfile)
            .DisposeWith(_disposable);
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
        _profiles.AddRange(profilesDataAccess.Profiles);
    }
    
    private void AddProfile(Profile profile)
    {
        _profiles.Add(profile);
    }

    private void RemoveProfile(Profile profile)
    {
	    bool isRemoved = _profiles.Remove(profile);
	    Guard.IsTrue(isRemoved);
    }
}