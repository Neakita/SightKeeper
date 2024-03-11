using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public interface ProfilesDataAccess
{
	IObservable<Profile> ProfileAdded { get; }
	IObservable<Profile> ProfileRemoved { get; }
	IReadOnlyCollection<Profile> Profiles { get; }

	void AddProfile(Profile profile);
	void RemoveProfile(Profile profile);
}