using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ProfilesDataAccess
{
    IObservable<Profile> ProfileAdded { get; }
    IObservable<Profile> ProfileRemoved { get; }

    Task AddProfile(Profile profile, CancellationToken cancellationToken = default);
    Task RemoveProfile(Profile profile, CancellationToken cancellationToken = default);
    Task UpdateProfile(Profile profile, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Profile>> LoadAllProfiles(CancellationToken cancellationToken = default);
    IObservable<Profile> LoadProfiles();
}