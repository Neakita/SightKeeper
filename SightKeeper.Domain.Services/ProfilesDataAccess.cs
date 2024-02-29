using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Domain.Services;

public interface ProfilesDataAccess
{
    IObservable<Profile> ProfileAdded { get; }
    IObservable<Profile> ProfileUpdated { get; }
    IObservable<Profile> ProfileRemoved { get; }

    Task AddProfile(Profile profile, CancellationToken cancellationToken = default);
    Task UpdateProfile(Profile profile, CancellationToken cancellationToken = default);
    Task RemoveProfile(Profile profile, CancellationToken cancellationToken = default);
    List<Profile> LoadProfiles();
    Task<List<Profile>> LoadProfilesAsync(CancellationToken cancellationToken = default);
}