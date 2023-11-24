using FluentValidation;
using SightKeeper.Domain.Services;

namespace SightKeeper.Application;

public sealed class NewProfileDataValidator : AbstractValidator<NewProfileData>
{
    private readonly ProfilesDataAccess _profilesDataAccess;

    public NewProfileDataValidator(IValidator<ProfileData> profileDataValidator, ProfilesDataAccess profilesDataAccess)
    {
        _profilesDataAccess = profilesDataAccess;
        Include(profileDataValidator);
        RuleFor(data => data.Name)
            .MustAsync(NameIsUnique);
    }

    private async Task<bool> NameIsUnique(string name, CancellationToken cancellationToken)
    {
        var profiles = await _profilesDataAccess.LoadProfilesAsync(cancellationToken);
        return profiles.All(profile => profile.Name != name);
    }
}