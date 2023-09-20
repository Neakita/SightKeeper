using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface EditedProfileData : ProfileData
{
    Profile? Profile { get; }
}