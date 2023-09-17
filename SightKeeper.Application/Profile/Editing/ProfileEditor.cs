using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ProfileEditor
{
    IObservable<Profile> ProfileEdited { get; }
}