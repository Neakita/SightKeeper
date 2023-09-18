using System.Reactive.Linq;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public sealed class ProfileEditor
{
    public IObservable<Profile> ProfileEdited { get; } = Observable.Empty<Profile>();
}