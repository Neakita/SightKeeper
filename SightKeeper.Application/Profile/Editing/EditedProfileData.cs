using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Application;

public interface EditedProfileData : ProfileData
{
    Profile? Profile { get; }
}