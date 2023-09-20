using FluentValidation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class EditedProfileDataValidator : AbstractValidator<EditedProfileData>
{
    public EditedProfileDataValidator(IValidator<ProfileData> profileDataValidator, ProfilesDataAccess profilesDataAccess)
    {
        _profilesDataAccess = profilesDataAccess;
        Include(profileDataValidator);
        RuleFor(data => data.Name)
            .MustAsync((data, _, cancellationToken) => NameIsUnique(data, cancellationToken));
    }

    private async Task<bool> NameIsUnique(EditedProfileData data, CancellationToken cancellationToken)
    {
        var profiles = await _profilesDataAccess.LoadAllProfiles(cancellationToken);
        return profiles.Where(profile => profile != data.Profile).All(profile => profile.Name != data.Name);
    }
    
    private readonly ProfilesDataAccess _profilesDataAccess;
}